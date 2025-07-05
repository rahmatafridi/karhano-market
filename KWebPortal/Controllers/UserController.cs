using BAL.Interfaces;
using BAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KWebPortal.Controllers
{
[Authorize(Roles = "SuperAdmin,StoreAdmin")]
public class UserController : Controller
{
        private readonly IUser _user;
        private readonly IRole _role;
        public UserController(IUser user, IRole role) { 
         _user = user;
            _role = role;
        }
        public IActionResult List()
        {

            return View(_user.GetUsers());
        }
        public IActionResult Add()
        {
            var roles = _role.GetRoles();
            var model = new UserVM();
            model.Roles = new List<RoleVM>();

            foreach (var item in roles)
            {
                model.Roles.Add(new RoleVM()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(UserVM model)
        {
            _user.AddUser(model);
            return RedirectToAction("List");
        }

        public IActionResult Edit(Guid Id)
        {
            var model = _user.GetUserById(Id);
            var roles = _role.GetRoles();

            model.Roles = new List<RoleVM>();
            foreach (var item in roles)
            {
                model.Roles.Add(new RoleVM()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(UserVM model)
        {
            _user.UpdateUser(model);
            return RedirectToAction("List");
        }

        public IActionResult Delete(Guid Id)
        {
            _user.DeleteUser(Id);
            return RedirectToAction("List");

        }


    }
}
