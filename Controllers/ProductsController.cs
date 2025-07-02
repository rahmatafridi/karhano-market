using KarhanoMarket.Models;
using KarhanoMarket.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarhanoMarket.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ICompanyRepository _companyRepository;

        public ProductsController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
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
    }
}
