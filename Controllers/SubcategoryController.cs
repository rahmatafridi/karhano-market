using KarhanoMarket.Models;
using KarhanoMarket.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace KarhanoMarket.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public SubcategoryController(ISubcategoryRepository subcategoryRepository, ICategoryRepository categoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var subcategories = await _subcategoryRepository.GetAllAsync();
            return View(subcategories);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                subcategory.Id = Guid.NewGuid();
                await _subcategoryRepository.AddAsync(subcategory);
                await _subcategoryRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var subcategory = await _subcategoryRepository.GetByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Subcategory subcategory)
        {
            if (id != subcategory.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _subcategoryRepository.Update(subcategory);
                await _subcategoryRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var subcategory = await _subcategoryRepository.GetByIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            return View(subcategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subcategory = await _subcategoryRepository.GetByIdAsync(id);
            if (subcategory != null)
            {
                _subcategoryRepository.Delete(subcategory);
                await _subcategoryRepository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
