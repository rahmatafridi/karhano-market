using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KWebPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _user;
        private readonly IPasswordHash _passwordHash;
        private readonly IStore _store;
        
        public AccountController(IUser user, IPasswordHash passwordHash, IStore store)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _passwordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        [HttpGet]
        public IActionResult Login()
        {
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleName),
                    new Claim("Name", user.Name),
                    new Claim("IsSuperAdmin", user.IsSuperAdmin.ToString()),
                    new Claim("IsStoreAdmin", user.IsStoreAdmin.ToString())
                };

                // Add store-specific claims if user is associated with a store
                if (user.StoreId.HasValue)
                {
                    claims.Add(new Claim("StoreId", user.StoreId.Value.ToString()));
                    claims.Add(new Claim("StoreName", user.StoreName ?? string.Empty));
                }

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
                HttpContext.Session.SetString("StoreId", user.StoreId.ToString());
                HttpContext.Session.SetString("IsSuperAdmin", user.IsSuperAdmin.ToString());

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the error here
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                return View(model);
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Impersonate(Guid userId)
        {
            try
            {
                // Only super admins can impersonate
                if (!User.HasClaim(c => c.Type == "IsSuperAdmin" && c.Value == "True"))
                {
                    return RedirectToAction("AccessDenied");
                }

                var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var targetUser = _user.GetUserById(userId);

                if (targetUser == null)
                {
                    return NotFound();
                }

                // Store original user info for reverting impersonation
                targetUser.ImpersonatedByUserId = currentUserId;
                targetUser.StoreId = currentUserId;

                // Create claims for impersonated user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, targetUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, targetUser.Email),
                    new Claim(ClaimTypes.Role, targetUser.RoleName),
                    new Claim("Name", targetUser.Name),
                    new Claim("IsImpersonating", "True"),
                    new Claim("StoreUserId", currentUserId.ToString()),
                    new Claim("IsSuperAdmin", targetUser.IsSuperAdmin.ToString()),
                    new Claim("IsStoreAdmin", targetUser.IsStoreAdmin.ToString())
                };

                if (targetUser.StoreId.HasValue)
                {
                    claims.Add(new Claim("StoreId", targetUser.StoreId.Value.ToString()));
                    claims.Add(new Claim("StoreName", targetUser.StoreName ?? string.Empty));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = false });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log error here
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StopImpersonating()
        {
            try
            {
                if (!User.HasClaim(c => c.Type == "IsImpersonating" && c.Value == "True"))
                {
                    return RedirectToAction("AccessDenied");
                }

                var originalUserId = Guid.Parse(User.FindFirstValue("OriginalUserId"));
                var originalUser = _user.GetUserById(originalUserId);

                if (originalUser == null)
                {
                    return NotFound();
                }

                // Create claims for original user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, originalUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, originalUser.Email),
                    new Claim(ClaimTypes.Role, originalUser.RoleName),
                    new Claim("Name", originalUser.Name),
                    new Claim("IsSuperAdmin", originalUser.IsSuperAdmin.ToString()),
                    new Claim("IsStoreAdmin", originalUser.IsStoreAdmin.ToString())
                };

                if (originalUser.StoreId.HasValue)
                {
                    claims.Add(new Claim("StoreId", originalUser.StoreId.Value.ToString()));
                    claims.Add(new Claim("StoreName", originalUser.StoreName ?? string.Empty));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties { IsPersistent = false });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log error here
                return RedirectToAction("Index", "Home");
            }
        }

      [Authorize(Roles = "SuperAdmin,StoreAdmin,User")]
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
              // Log error here
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
