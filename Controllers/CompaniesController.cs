using KarhanoMarket.Models;
using KarhanoMarket.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarhanoMarket.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(ICompanyRepository companyRepository, UserManager<ApplicationUser> userManager)
        {
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var companies = await _companyRepository.GetActiveCompaniesAsync();
            return View(companies);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company, string adminEmail, string adminPassword, string adminFullName)
        {
            if (ModelState.IsValid)
            {
                company.Id = Guid.NewGuid();
                await _companyRepository.AddAsync(company);
                await _companyRepository.SaveChangesAsync();

                // Create admin user for the company
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = adminFullName,
                    CompanyId = company.Id,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Delete the company if user creation failed
                    _companyRepository.Delete(company);
                    await _companyRepository.SaveChangesAsync();

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(company);
        }
    }
}
