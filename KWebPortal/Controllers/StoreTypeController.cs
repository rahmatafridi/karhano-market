using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Mvc;

namespace KWebPortal.Controllers
{
    public class StoreTypeController : Controller
    {
        private readonly IStoreType _storeType;
        public StoreTypeController(IStoreType storeType) { 
       _storeType = storeType;  
        }    
        public IActionResult List()
        {
            
            return View(_storeType.GetStoreTypes());
        }
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(StoreTypeModel storeType)
        {
            _storeType.AddStoreType(storeType);
            return RedirectToAction("List");
        }


        public IActionResult Edit(Guid Id)
        {
            var data = _storeType.GetStoreType(Id);
            return View(data);
        }


        [HttpPost]
        public IActionResult Edit(StoreTypeModel storeType)
        {
            _storeType.UpdateStoreType(storeType);
            return RedirectToAction("List");
        }
        public IActionResult Delete(Guid Id)
        {
            _storeType.DeleteStoreType(Id);
            return RedirectToAction("List");

        }
    }
}
