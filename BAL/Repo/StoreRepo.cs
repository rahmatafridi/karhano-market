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
    public class StoreRepo : IStore
    {
        private readonly KWebContext _context;
        public StoreRepo(KWebContext context) {
        _context = context; 
        }
        public Guid AddStore(StoreModel model)
        {
            try
            {
                var store = new Store();
                store.Address = model.Address;
                store.Name = model.Name;    
                store.Status = model.Status;    
                store.Email = model.Email;  
                store.ContactNo = model.ContactNo;  
                store.ContactPerson = model.ContactPerson;  
                store.CreatedBy = model.CreatedBy;  
                store.Description = model.Description;  
                store.Id = Guid.NewGuid();
                store.OwnerName = model.OwnerName;  
                store.TypeId = model.TypeId;
                store.CreatedDate = DateTime.Now;

                _context.Add(store);
                _context.SaveChanges();
                return store.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool DeleteStore(Guid Id)
        {
            var data = _context.Stores.Where(x => x.Id == Id).FirstOrDefault();
            if (data !=null)
            {
                _context.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false; 
        }

        public StoreModel GetStore(Guid id)
        {
            var store = _context.Stores.FirstOrDefault(s => s.Id == id);
            if (store == null) return null;

            var type = _context.StoreTypes.Where(x => x.Id == store.TypeId).FirstOrDefault();

            return new StoreModel
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                Address = store.Address,
                TypeId = store.TypeId,
                StoreType = type.Name,
                Email = store.Email,
                ContactNo = store.ContactNo,
                ContactPerson = store.ContactPerson,
                OwnerName = store.OwnerName,
                CreatedDate = store.CreatedDate,
                UpdatedDate = store.UpdatedDate,
                CreatedBy = store.CreatedBy,
                UpdatedBy = store.UpdatedBy,
                Status = store.Status
            };
        }

        public List<StoreModel> GetStores()
        {
            return _context.Stores.Select(store => new StoreModel
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                Address = store.Address,
                TypeId = store.TypeId,
                Email = store.Email,
                ContactNo = store.ContactNo,
                ContactPerson = store.ContactPerson,
                OwnerName = store.OwnerName,
                CreatedDate = store.CreatedDate,
                UpdatedDate = store.UpdatedDate,
                CreatedBy = store.CreatedBy,
                UpdatedBy = store.UpdatedBy,
                Status = store.Status,
                StoreType = _context.StoreTypes.Where(x => x.Id == store.TypeId).FirstOrDefault().Name

            }).ToList();
        }

        public bool UpdateStore(StoreModel storeModel)
        {
            var store = _context.Stores.FirstOrDefault(s => s.Id == storeModel.Id);
            if (store == null)
                return false;

            store.Name = storeModel.Name;
            store.Description = storeModel.Description;
            store.Address = storeModel.Address;
            store.TypeId = storeModel.TypeId;
            store.Email = storeModel.Email;
            store.ContactNo = storeModel.ContactNo;
            store.ContactPerson = storeModel.ContactPerson;
            store.OwnerName = storeModel.OwnerName;
            store.UpdatedDate = DateTime.UtcNow;
            store.UpdatedBy = storeModel.UpdatedBy;
            store.Status = storeModel.Status;

            _context.Stores.Update(store);
            _context.SaveChanges();
            return true;
        }
    }
}
