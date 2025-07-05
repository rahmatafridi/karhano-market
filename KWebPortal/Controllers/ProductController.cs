using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace KWebPortal.Controllers
{
[Authorize(Roles = "SuperAdmin,StoreAdmin")]
public class ProductController : Controller
{
        protected KWebContext _context;
        protected IProduct _product;
        protected ICategory _category;
        protected IStore _store;
        protected ISubCategory _subCategory;

        public ProductController(KWebContext context , IProduct product, ICategory category , IStore store , ISubCategory subcategory )
        {

            _context = context;
            _product = product;
            _category = category;
            _store = store;
            _subCategory = subcategory;
        }

        public IActionResult List()
        {
            var storeId = HttpContext.Session.GetString("StoreId");

            var data = _product.GetProducts(Guid.Parse(storeId));
            return View(data);
        }

        public IActionResult Add()
        {

            var categories = _category.GetCategories();
            var subcategories = _subCategory.GetSubCategory();
            var store = _store.GetStores();

            var model = new ProductVM();
            model.Categories = new List<CategoryVM>();
            model.SubCategories = new List<SubCategoryVM>();
            model.Stores = new List<StoreModel>();
            foreach (var item in categories)
            {
                model.Categories.Add(new CategoryVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            };
            foreach (var item in subcategories)
            {
                model.SubCategories.Add(new SubCategoryVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            };
            foreach (var item in store)
            {
                model.Stores.Add(new StoreModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(ProductVM model)
        {
            var storeId = HttpContext.Session.GetString("StoreId");
            model.StoreId = Guid.Parse(storeId);
            model.Status = true;

            // Set CreatedBy from current user claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                model.CreatedBy = userId;
            }

            var productId = _product.AddProduct(model);

            var images = new List<ProductImageVM>();
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            if (model.Images != null && model.Images.Any())
            {
                foreach (var file in model.Images)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var savePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        images.Add(new ProductImageVM
                        {
                            Id = Guid.NewGuid(),
                            Name = fileName,
                            FilePath = Path.Combine("uploads", fileName).Replace("\\", "/"),
                            ProductId = productId
                        });
                    }
                }

                _product.UploadImages(images);
            }

            return RedirectToAction("List");
        }



        public IActionResult Edit(Guid Id)
        {
            var categories = _category.GetCategories();
            var subcategories = _subCategory.GetSubCategory();
            var store = _store.GetStores();

            var model = _product.GetProduct(Id);
            model.Categories = new List<CategoryVM>();
            model.SubCategories = new List<SubCategoryVM>();
            model.Stores = new List<StoreModel>();
            foreach (var item in categories)

            {
                model.Categories.Add(new CategoryVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            ;
            foreach (var item in subcategories)
            {
                model.SubCategories.Add(new SubCategoryVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            ;
            foreach (var item in store)
            {
                model.Stores.Add(new StoreModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM model)
        {
            _product.UpdateProduct(model);
            return RedirectToAction("List");
        }

        public IActionResult Delete(Guid Id)
        {

            var data = _product.DeleteProduct(Id);

            return RedirectToAction("List");

        }

        public JsonResult SubCategories(Guid Id)
        {
            var data = _subCategory.GetSubCategoryByCategoryId(Id);
            return new JsonResult(data);
        }

    }

    
}
