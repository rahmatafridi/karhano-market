using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KWebPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _user;
        private readonly IPasswordHash _passwordHash;
        
        public AccountController(IUser user, IPasswordHash passwordHash)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _passwordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If user is already logged in, redirect to home
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _user.GetUserByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Invalid email or password");
                    return View(model);
                }

                if (!user.Status)
                {
                    ModelState.AddModelError("", "Your account is disabled. Please contact administrator.");
                    return View(model);
                }

                bool isValid = _passwordHash.VerifyPassword(user.Password, model.Password);
                if (!isValid)
                {
                    ModelState.AddModelError("Password", "Invalid email or password");
                    return View(model);
                }

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTime.UtcNow.AddHours(24)
                    });

                // Store minimal information in session
                HttpContext.Session.SetString("UserName", user.Email);
                HttpContext.Session.SetString("Name", user.Name);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the error here
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                // Clear session
                HttpContext.Session.Clear();
                
                // Clear authentication cookies
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                // Log the error here
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
