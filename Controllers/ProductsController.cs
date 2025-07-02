using KarhanoMarket.Models;
using KarhanoMarket.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KarhanoMarket.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public ProductsController(ICompanyRepository companyRepository, IGenericRepository<Product> productRepository)
        {
            _companyRepository = companyRepository;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = User;
            var companyIdClaim = user.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim))
            {
                // SuperAdmin: show all products
                var companies = await _companyRepository.GetActiveCompaniesAsync();
                var allProducts = new List<Product>();
                foreach (var company in companies)
                {
                    var products = await _companyRepository.GetCompanyProductsAsync(company.Id);
                    allProducts.AddRange(products);
                }
                return View(allProducts);
            }
            else
            {
                Guid companyId = Guid.Parse(companyIdClaim);
                var products = await _companyRepository.GetCompanyProductsAsync(companyId);
                return View(products);
            }
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var companyIdClaim = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyIdClaim))
                {
                    ModelState.AddModelError(string.Empty, "Company ID is missing.");
                    return View(product);
                }

                product.Id = Guid.NewGuid();
                product.CompanyId = Guid.Parse(companyIdClaim);
                product.CreatedAt = DateTime.UtcNow;

                await _productRepository.AddAsync(product);
                await _productRepository.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}
