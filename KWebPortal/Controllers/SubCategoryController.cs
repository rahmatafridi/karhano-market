using BAL.Interfaces;
using BAL.Model;
using DAL.Enities;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace KWebPortal.Controllers
{
    public class SubCategoryController : Controller
    {
        private ISubCategory _subcategory;
        private ICategory _category;
        public SubCategoryController(ISubCategory subcategory,ICategory category)
        {
            _subcategory = subcategory;
            _category = category;

        }

        public IActionResult List()
        {
            var data = _subcategory.GetSubCategory();
            return View(data);
        }

        public IActionResult Add()
        {
            var categories = _category.GetCategories();
            var model = new SubCategoryVM();
            model.Categories = new List<CategoryVM>();
            foreach (var item in categories)
            {
                model.Categories.Add(new CategoryVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            return View(model);


        }

        [HttpPost]
        public IActionResult Add(SubCategoryVM subcategory)
        {
            _subcategory.AddSubCategory(subcategory);
            return RedirectToAction("List");
        }

        public IActionResult Edit(Guid Id)
        {
            var categories = _category.GetCategories();
            var model = _subcategory.GetSubCategory(Id);
            model.Categories = new List<CategoryVM>();
            foreach (var item in categories)
            {
                model.Categories.Add(new CategoryVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(SubCategoryVM model)
        {
            _subcategory.UpdateSubCategory(model);
            return RedirectToAction("List");
        }


        public IActionResult Delete(Guid Id)
        {

            var data = _subcategory.DeleteSubCategory(Id);

            return RedirectToAction("List");

        }




    }
}
