using KarhanoMarket.Models;
using KarhanoMarket.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarhanoMarket.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyRepository _companyRepository;

        public UsersController(UserManager<ApplicationUser> userManager, ICompanyRepository companyRepository)
        {
            _userManager = userManager;
            _companyRepository = companyRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            var userList = new List<(ApplicationUser user, IList<string> roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add((user, roles));
            }

            return View(userList);
        }
    }
}
