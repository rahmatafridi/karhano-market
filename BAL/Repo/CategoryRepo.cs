using BAL.Interfaces;
using DAL.Enities;
using BAL.Model;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repo
{
    public class CategoryRepo : ICategory
    {
        private readonly KWebContext _context;
        public CategoryRepo(KWebContext context)
        {
            _context = context;
        }

        public Guid AddCategory(CategoryVM model)
        {
            try
            {
                var category = new Category();
                category.Status = model.Status;
                category.Id = Guid.NewGuid();
                category.CreatedDate = model.CreatedDate;
                category.UpdatedDate = model.UpdatedDate;
                category.CreatedBy = model.CreatedBy;
                category.UpdatedBy = model.UpdatedBy;
                category.Name = model.Name;
                _context.Add(category);
                _context.SaveChanges();
                return category.Id;


            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public bool DeleteCategory(Guid Id)
        {
            var data = _context.Categories.Where(x => x.Id == Id).FirstOrDefault();
            if (data != null)
            {
                _context.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<CategoryVM> GetCategories()
        {
            return _context.Categories.Select(category => new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,

                CreatedDate = category.CreatedDate,
                UpdatedDate = category.UpdatedDate,
                CreatedBy = category.CreatedBy,
                UpdatedBy = category.UpdatedBy,
                Status = category.Status,


            }).ToList();
        }

        public CategoryVM GetCategoryById(Guid Id)
        {
            var data = _context.Categories.FirstOrDefault(x => x.Id == Id);
            return new CategoryVM
            {
                Id = data.Id,
                Name = data.Name,
                CreatedDate = data.CreatedDate,
                UpdatedDate = data.UpdatedDate,
                CreatedBy = data.CreatedBy,
                UpdatedBy = data.UpdatedBy,
                Status = data.Status,
            };
        }

        public bool UpdateCategory(CategoryVM category)
        {
            var data = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (data != null)
            {
                data.Status = category.Status;
                data.Id = category.Id;
                data.CreatedDate = category.CreatedDate;
                data.UpdatedDate = category.UpdatedDate;
                data.CreatedBy = category.CreatedBy;
                data.UpdatedBy = category.UpdatedBy;
                data.Name = category.Name;
                _context.Update(data);
                _context.SaveChanges();
                return true;


            }
            return false;
        }
    }
}
        


    

