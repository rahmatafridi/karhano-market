using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repo
{
    public class UserRepo : IUser
    {
        private readonly KWebContext _context;
        private readonly IPasswordHash _passwordHash;
        public UserRepo(KWebContext kWebContext,IPasswordHash passwordHash) { 
         _context = kWebContext;    
            _passwordHash = passwordHash;
        }

        public bool AddUser(UserVM model)
        {
            try
            {
                var user = new User();
                user.Status = model.Status;
                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = _passwordHash.HashPassword(model.Password);
                user.Id = Guid.NewGuid();
                user.RoleId = model.RoleId;
                user.StoreId = model.StoreId;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = model.CreatedBy;
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool DeleteUser(Guid Id)
        {
            var data = _context.Users.FirstOrDefault(x => x.Id == Id);
            if (data != null)
            {
                _context.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public UserVM GetUserByEmail(string email)
        {
            try
            {

                var data = _context.Users.FirstOrDefault(x => x.Email == email);
                var model = new UserVM();
                if (data != null)
                {
                    model.Id = data.Id;
                    model.Name = data.Name;
                    model.Email = data.Email;
                    model.Password = data.Password;
                    model.Status = data.Status;
                    model.RoleId = data.RoleId;
                    model.StoreId = data.StoreId;
                }
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public UserVM GetUserById(Guid Id)
        {
            try
            {
                var data = _context.Users.FirstOrDefault(x => x.Id == Id);
                var model = new UserVM();
                if (data != null)
                {
                    model.Id = data.Id;
                    model.Name = data.Name;
                    model.Email = data.Email;
                    model.Password = data.Password;
                    model.Status = data.Status;
                    model.RoleId = data.RoleId;
                    model.StoreId = data.StoreId;
                }
                return model;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<UserVM> GetUsers()
        {
            var userlistdata = new List<UserVM>();  
            var users = _context.Users.ToList();
            foreach (var item in users)
            {
                userlistdata.Add(new UserVM()
                {
                    Email = item.Email,
                    Id = item.Id,   
                    Name = item.Name,   
                    Password = item.Password,   
                    RoleId = item.RoleId,
                    Status = item.Status,   
                   StoreId = item.StoreId
                });
            }
            return userlistdata;
        }

        public bool UpdateUser(UserVM user)
        {
            try
            {
                var checkData = _context.Users.FirstOrDefault(x => x.Id == user.Id);
                if (checkData != null)
                {
                    checkData.Email = user.Email;
                    checkData.Status = user.Status;
                    checkData.Name = user.Name;
                    checkData.Password = user.Password;
                    checkData.Id = user.Id;
                    checkData.StoreId = user.StoreId;

                    _context.Update(checkData);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
