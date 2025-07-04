using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BAL.Repo
{
    public class SubCategoryRepo : ISubCategory
    {
        private KWebContext _context;

        public SubCategoryRepo(KWebContext context)
        {
            _context = context;
        }

        public Guid AddSubCategory(SubCategoryVM model)
        {
            try
            {
                var data = new SubCategory();
                data.Id = Guid.NewGuid();
                data.Name = model.Name;
                data.CreatedBy = model.CreatedBy;
                data.UpdatedBy = model.UpdatedBy;
                data.Status = model.Status;
                data.CreatedDate = model.CreatedDate;
                data.UpdatedDate = model.UpdatedDate;
                data.CategoryId = model.CategoryId;
                _context.SubCategories.Add(data);
                _context.SaveChanges();
                return data.Id;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public bool DeleteSubCategory(Guid Id)
        {
            try
            {
                var data = _context.SubCategories.Where(x => x.Id == Id).FirstOrDefault();
                if (data != null)
                {
                    _context.Remove(data);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public List<SubCategoryVM> GetByCategoryId(Guid Id)
        {
            
           try
            {
                var data = _context.SubCategories.Where(x => x.Id == x.CategoryId);
                var model = new List<SubCategoryVM>();

                foreach (var item in data)
                {
                    model.Add(new SubCategoryVM()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Status = item.Status,
                        CreatedDate = item.CreatedDate,
                        CreatedBy = item.CreatedBy,
                        CategoryId = item.CategoryId,
                        UpdatedBy = item.UpdatedBy

                    });
                }

                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<SubCategoryVM> GetSubCategoryByCategoryId(Guid Id)
        {
            try
            {
                return _context.SubCategories
                    .Where(x => x.CategoryId == Id)
                    .Select(item => new SubCategoryVM
                    {
                        Id = item.Id,
                        Name = item.Name
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public List<SubCategoryVM> GetSubCategory()
        {
            try
            {
                var model = new List<SubCategoryVM>();
                var data = _context.SubCategories.ToList();

                foreach (var item in data)
                {
                    var cate = "";
                    var category = _context.Categories.Where(x => x.Id == item.CategoryId).FirstOrDefault();
                    if(category == null)
                    {
                        cate = "";
                    }
                    else
                    {
                        cate = category.Name;
                    }

                        model.Add(new SubCategoryVM()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Status = item.Status,
                            CreatedDate = item.CreatedDate,
                            CreatedBy = item.CreatedBy,
                            CategoryId = item.CategoryId,
                            UpdatedBy = item.UpdatedBy,
                            Category = cate

                        });
                }

                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SubCategoryVM GetSubCategory(Guid Id)
        {
            try
            {
                var data = _context.SubCategories.Where(x => x.Id == Id).FirstOrDefault();
                var model = new SubCategoryVM();
                if (data != null)
                {
                    model.Id = data.Id;
                    model.Name = data.Name;
                    model.Status = data.Status;
                    model.CreatedDate = data.CreatedDate;
                    model.CreatedBy = data.CreatedBy;
                    model.CategoryId = data.CategoryId;
                    model.UpdatedBy = data.UpdatedBy;
                }
                return model;
            }
            catch(Exception ex)
            {
                throw;

            }


        }

        public bool UpdateSubCategory(SubCategoryVM model)
        {
            var data = _context.SubCategories.Where(x => x.Id == model.Id).FirstOrDefault();
            if (data != null)
            {
                data.Id = model.Id;
                data.Name = model.Name;
                data.Status = model.Status;
                data.CreatedDate = model.CreatedDate;
                data.CreatedBy = model.CreatedBy;
                data.CategoryId = model.CategoryId;
                data.UpdatedBy = model.UpdatedBy;
                 _context.SubCategories.Update(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
