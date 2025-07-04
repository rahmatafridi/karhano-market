using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace KWebPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _user;
        private readonly IPasswordHash _passwordHash;
        public  AccountController(IUser user ,IPasswordHash passwordHash)
        {
            _user = user;   
            _passwordHash = passwordHash;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            var user = _user.GetUserByEmail(model.Email);
            if (user != null) {
                bool isValid = _passwordHash.VerifyPassword(user.Password, model.Password);

                if (isValid)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Email) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principle = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
                    HttpContext.Session.SetString("UserName", model.Email);
                    HttpContext.Session.SetString("name", user.Name);
                    HttpContext.Session.SetString("RoleId", user.RoleId.ToString());
                    return RedirectToAction("Index", "Home");


                }
                else
                {
                    TempData["errorpassword"] = "Invalid Password";
                    return View(model);
                }

                }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedcookies = Request.Cookies.Keys;
            foreach (var cokkie in storedcookies)
            {
                Response.Cookies.Delete(cokkie);
            }
            return RedirectToAction("LogIn", "Account");
        }
    }
}
