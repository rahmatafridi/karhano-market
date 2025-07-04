using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repo
{
    public class RoleRepo : IRole
    {
        private readonly KWebContext _context;
        public RoleRepo(KWebContext context) { 
         _context = context;
        }   
        public bool AddRole(RoleVM model)
        {
            try
            {
                var role = new Role();
                role.Id = Guid.NewGuid();   
                role.Status = model.Status;
                role.Name = model.Name;
               _context.Roles.Add(role);    
                _context.SaveChanges(); 
                return true;
            }
            catch (Exception ex) 
            {

                throw ex;
            }
        }

        public bool DeleteRole(Guid Id)
        {
            try
            {
                var checkRole = _context.Roles.FirstOrDefault(x => x.Id == Id);
                if (checkRole != null)
                {
                    _context.Remove(checkRole);
                    _context.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public RoleVM GetRoleById(Guid id)
        {
            try
            {
                var data = _context.Roles.Where(x => x.Id == id).FirstOrDefault();
                var model = new RoleVM();
                model.Id = data.Id;
                model.Name = data.Name; 
                model.Status = data.Status;

                return model;   
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<RoleVM> GetRoles()
        {
            try
            {
                var datalist = _context.Roles.Where(x => x.Status == true && x.Name !="SuperAdmin").ToList();
                var model = new List<RoleVM>();
                foreach (var item in datalist)
                {
                    model.Add(new RoleVM()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Status = item.Status
                    });
                }
                return model;   
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool UpdateRole(RoleVM role)
        {
            var existingdata= _context.Roles.FirstOrDefault(x=>x.Id == role.Id);
            if(existingdata != null)
            {
                existingdata.Status = role.Status;
                existingdata.Name = role.Name;
               _context.Update(existingdata);
                _context.SaveChanges(); 
                return true;
            }
            return false;
        }
    }
}
