using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public  interface IStoreType
    {
        List<StoreTypeModel> GetStoreTypes();
        bool AddStoreType(StoreTypeModel model);
        bool UpdateStoreType (StoreTypeModel model);    
        StoreTypeModel GetStoreType (Guid id);
        bool DeleteStoreType (Guid id);
    }
}
