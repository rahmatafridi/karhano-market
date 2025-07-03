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
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly IImageRepository _imageRepository;

        public ProductsController(
            ICompanyRepository companyRepository,
            IGenericRepository<Product> productRepository,
            ICategoryRepository categoryRepository,
            ISubcategoryRepository subcategoryRepository,
            IImageRepository imageRepository)
        {
            _companyRepository = companyRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
            _imageRepository = imageRepository;
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
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "Id", "Name");
            ViewBag.Subcategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(Enumerable.Empty<object>(), "Id", "Name");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                var companyIdClaim = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(companyIdClaim))
                {
                    ModelState.AddModelError(string.Empty, "Company ID is missing.");
                    var categories = await _categoryRepository.GetAllAsync();
                    var subcategories = await _subcategoryRepository.GetAllAsync();

                    ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "Id", "Name");
                    ViewBag.Subcategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(subcategories, "Id", "Name");

                    return View(product);
                }

                product.Id = Guid.NewGuid();
                product.CompanyId = Guid.Parse(companyIdClaim);
                product.CreatedAt = DateTime.UtcNow;

                await _productRepository.AddAsync(product);
                await _productRepository.SaveChangesAsync();

                // Handle image uploads
                if (images != null && images.Count > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products", product.Id.ToString());
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var image in images)
                    {
                        if (image.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var imagePath = Path.Combine("uploads", "products", product.Id.ToString(), fileName).Replace("\\", "/");

                            var productImage = new Image
                            {
                                Id = Guid.NewGuid(),
                                ProductId = product.Id,
                                ImagePath = imagePath,
                                CreatedAt = DateTime.UtcNow
                            };

                            await _imageRepository.AddAsync(productImage);
                        }
                    }
                    await _imageRepository.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            var allCategories = await _categoryRepository.GetAllAsync();
            var allSubcategories = await _subcategoryRepository.GetAllAsync();

            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(allCategories, "Id", "Name");
            ViewBag.Subcategories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(allSubcategories, "Id", "Name");

            return View(product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var images = await _imageRepository.GetAllAsync();
            var productImages = images.FindAll(img => img.ProductId == product.Id);

            ViewBag.ProductImages = productImages;

            return View(product);
        }
    }
}
