using KarhanoMarket.Models;
using KarhanoMarket.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KarhanoMarket.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ImpersonationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICompanyRepository _companyRepository;

        public ImpersonationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _companyRepository = companyRepository;
        }

        [HttpPost]
        public async Task<IActionResult> StartImpersonation(string userId)
        {
            var userToImpersonate = await _userManager.FindByIdAsync(userId);
            if (userToImpersonate == null)
            {
                return NotFound();
            }

            // Sign out current user
            await _signInManager.SignOutAsync();

            // Create new identity with impersonation claim
            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(userToImpersonate);
            var identity = (ClaimsIdentity)userPrincipal.Identity;

            // Add impersonation claims
            identity.AddClaim(new Claim("IsImpersonating", "true"));
            identity.AddClaim(new Claim("ImpersonatorId", User.FindFirstValue(ClaimTypes.NameIdentifier)));

            // Sign in as impersonated user
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, userPrincipal);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> StopImpersonation()
        {
            var impersonatorId = User.FindFirstValue("ImpersonatorId");
            if (string.IsNullOrEmpty(impersonatorId))
            {
                return BadRequest();
            }

            var impersonator = await _userManager.FindByIdAsync(impersonatorId);
            if (impersonator == null)
            {
                return NotFound();
            }

            // Sign out current impersonated user
            await _signInManager.SignOutAsync();

            // Sign back in as impersonator
            await _signInManager.SignInAsync(impersonator, isPersistent: false);

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
