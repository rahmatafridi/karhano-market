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
    public class StoreTypeRepo : IStoreType
    {
        private KWebContext _context;
        public StoreTypeRepo(KWebContext context) { 
        _context = context;
        }

        public bool AddStoreType(StoreTypeModel model)
        {
            try
            {
                var storetype = new StoreType();
                storetype.Name = model.Name;
                storetype.Id = Guid.NewGuid();
                storetype.CreatedBy= model.CreatedBy;
                storetype.CreatedDate = DateTime.Now;
                storetype.Status = model.Status;
                _context.Add(storetype);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool DeleteStoreType(Guid id)
        {
            try
            {
                var data = _context.StoreTypes.Where(x => x.Id == id).FirstOrDefault();
                if (data != null) {
                    _context.Remove(data);
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

        public StoreTypeModel GetStoreType(Guid id)
        {
            try
            {
                var model = new StoreTypeModel();
                var data = _context.StoreTypes.Where(x => x.Id == id).FirstOrDefault();
                if (data != null) { 
                 model.Id= data.Id;
                 model.Name = data.Name;
                 model.Status = data.Status;    
                 model.CreatedDate = data.CreatedDate;
                 model.CreatedBy = data.CreatedBy;
                 return model;
                
                }
                return model;   
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<StoreTypeModel> GetStoreTypes()
        {

            try
            {
                var model = new List<StoreTypeModel>();
                var data = _context.StoreTypes.ToList();

                foreach (var item in data)
                {
                    model.Add(new StoreTypeModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Status = item.Status,
                        CreatedDate = item.CreatedDate,
                        CreatedBy = item.CreatedBy

                    });
                }
               
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateStoreType(StoreTypeModel model)
        {
            try
            {
                var data = _context.StoreTypes.Where(x => x.Id == model.Id).FirstOrDefault();
                if (data != null) {
                    data.Name = model.Name;
                    data.UpdatedBy = model.UpdatedBy;
                    data.Status = model.Status;
                    data.UpdatedDate = DateTime.Now;
                    _context.Update(data);
                    _context.SaveChanges();
                    return true;
                
                }
                return false;
            }
            catch (Exception EX)
            {

                throw EX;
            }
        }
    }
}
