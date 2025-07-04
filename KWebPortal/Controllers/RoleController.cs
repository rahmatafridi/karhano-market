using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KWebPortal.Controllers
{
    //[Authorize]

    public class RoleController : Controller
    {

        private readonly IRole _role;
        public RoleController(IRole role) { 
        _role = role;   
        }
        // GET: RoleController
        public ActionResult List()
        {
            var modeldata= _role.GetRoles();
            return View(modeldata);
        }
       
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(RoleVM model)
        {
            _role.AddRole(model);
            return RedirectToAction("List");
        }

        public IActionResult Edit(Guid Id)
        {
            var data = _role.GetRoleById(Id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(RoleVM roleVM)
        {
            _role.UpdateRole(roleVM);
            return RedirectToAction("List");

        }

        public IActionResult Delete(Guid Id)
        {
            var data = _role.DeleteRole(Id);
            return RedirectToAction("List");
        }

    }
}
