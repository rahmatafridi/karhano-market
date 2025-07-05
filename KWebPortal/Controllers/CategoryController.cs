using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using Microsoft.AspNetCore.Mvc;

namespace KWebPortal.Controllers
{
[Authorize(Roles = "SuperAdmin,StoreAdmin")]
public class CategoryController : Controller
{
        protected readonly KWebContext _context;
        private readonly ICategory _category;

        public CategoryController(KWebContext context,ICategory category)
        {
            _context = context;
            _category = category;
        }

        public IActionResult List()
        {
            var data = _category.GetCategories();

            return View(data);
        }

        public IActionResult Add()
        {
            var model = new CategoryVM();

            return View(model);
        }
        [HttpPost]
        public IActionResult Add(CategoryVM model) {

            _category.AddCategory(model);
         
        return RedirectToAction("List");
    }

        public IActionResult Delete(Guid Id)
        {
            var data = _category.DeleteCategory(Id);

            return RedirectToAction("List");
        }

        public IActionResult Edit(Guid Id)
        {
            var data = _category.GetCategoryById(Id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(CategoryVM model)
        {
            _category.UpdateCategory(model);
            return RedirectToAction("List");

        }




    }
}
