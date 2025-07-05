using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAL.Repo
{
    public class UserRepo : IUser
    {
        private readonly KWebContext _context;
        private readonly IPasswordHash _passwordHash;

        public UserRepo(KWebContext kWebContext, IPasswordHash passwordHash)
        {
            _context = kWebContext ?? throw new ArgumentNullException(nameof(kWebContext));
            _passwordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }

        public bool AddUser(UserVM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            try
            {
                // Check if email already exists
                if (_context.Users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
                {
                    throw new InvalidOperationException("Email already exists");
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Status = model.Status,
                    Name = model.Name?.Trim(),
                    Email = model.Email?.ToLower().Trim(),
                    Password = _passwordHash.HashPassword(model.Password),
                    RoleId = model.RoleId,
                    StoreId = model.StoreId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "",
                    LastLoginIP ="",
                    ModifiedBy=""
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Log exception here
                return false;
            }
        }

        public bool DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(id));

            try
            {
                var user = _context.Users.Find(id);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Log exception here
                return false;
            }
        }

        public UserVM GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            try
            {
                var user = _context.Users
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

                if (user == null)
                    return null;

                bool isSupperAdmin = false;
                bool isStoreAdmin = false;
                var role = _context.Roles.Where(x => x.Id == user.RoleId).FirstOrDefault();

                if(role.Name == "StoreAdmin")
                {
                    isStoreAdmin = true;
                }
                if(role.Name == "SuperAdmin")
                {
                    isSupperAdmin = true;
                }
                return new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Status = user.Status,
                    RoleId = user.RoleId,
                    StoreId = user.StoreId,
                    IsSuperAdmin = isSupperAdmin,
                    IsStoreAdmin = isStoreAdmin,
                    RoleName = role.Name
                };
            }
            catch (Exception)
            {
                // Log exception here
                return null;
            }
        }

        public UserVM GetUserById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(id));

            try
            {
                var user = _context.Users
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);

                if (user == null)
                    return null;

                return new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Status = user.Status,
                    RoleId = user.RoleId,
                    StoreId = user.StoreId
                };
            }
            catch (Exception)
            {
                // Log exception here
                return null;
            }
        }

        public List<UserVM> GetUsers()
        {
            try
            {
                return _context.Users
                    .AsNoTracking()
                    .Select(user => new UserVM
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Password = user.Password,
                        RoleId = user.RoleId,
                        Status = user.Status,
                        StoreId = user.StoreId
                    })
                    .ToList();
            }
            catch (Exception)
            {
                // Log exception here
                return new List<UserVM>();
            }
        }

        public bool UpdateUser(UserVM model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(model.Id));

            try
            {
                var user = _context.Users.Find(model.Id);
                if (user == null)
                    return false;

                // Check if email is being changed and if it already exists
                if (user.Email != model.Email && 
                    _context.Users.Any(u => u.Email.ToLower() == model.Email.ToLower() && u.Id != model.Id))
                {
                    throw new InvalidOperationException("Email already exists");
                }

                user.Email = model.Email?.ToLower().Trim();
                user.Status = model.Status;
                user.Name = model.Name?.Trim();
                // Only update password if it's provided
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.Password = _passwordHash.HashPassword(model.Password);
                }
                user.StoreId = model.StoreId;
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = model.ModifiedBy;

                _context.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Log exception here
                return false;
            }
        }
    }
}
