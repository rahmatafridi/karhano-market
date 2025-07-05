using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KWebPortal.Controllers
{
[Authorize(Roles = "SuperAdmin,StoreAdmin")]
public class StoreController : Controller
{
        private readonly IStore _store;
        private readonly IStoreType _storeType;
        private readonly IUser _user;
        public StoreController(IStore store,IStoreType storeType,IUser user) { 
         _store = store;    
           _storeType = storeType;
            _user = user;
        }
        public ActionResult List()
        {
            
            return View(_store.GetStores());
        }
        public IActionResult Add()
        {
            var model = new StoreModel(); 
            var typelist = new List<StoreTypeModel>();
             var types= _storeType.GetStoreTypes();
            foreach (var item in types)
            {
                typelist.Add(new StoreTypeModel()
                {
                    Id = item.Id,   
                    Name = item.Name,
                });
            }
            model.StoreTypes = typelist;
            return View(model);

        }

        [HttpPost]
        public IActionResult Add(StoreModel model)
        {
           var storeId= _store.AddStore(model);

            var usermodel = new UserVM();
            usermodel.Password = GeneratePassword();
            usermodel.Email = model.Email;
            usermodel.Status = true ;
            usermodel.RoleId = Guid.Parse("5EF39EF1-037F-4DCC-82A4-2CBB30470AFA");
            usermodel.StoreId = storeId;
            usermodel.Name = model.OwnerName;
            _user.AddUser(usermodel); 

            return RedirectToAction("List");

        }

        public IActionResult Edit(Guid Id) {
            var model = _store.GetStore(Id);
            var typelist = new List<StoreTypeModel>();
            var types = _storeType.GetStoreTypes();
            foreach (var item in types)
            {
                typelist.Add(new StoreTypeModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }
            model.StoreTypes = typelist;
            return View(model);

        }

        [HttpPost]

        public IActionResult Edit(StoreModel model)
        {
            _store.UpdateStore(model);  
            return RedirectToAction("List");

        }
        public IActionResult Delete(Guid Id)
        {
            _store.DeleteStore(Id);
            return RedirectToAction("List");


        }

        public static string GeneratePassword(int length = 8)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
            Random rnd = new Random();
            return new string(Enumerable.Range(1, length)
                .Select(_ => validChars[rnd.Next(validChars.Length)]).ToArray());
        }
    }
}
