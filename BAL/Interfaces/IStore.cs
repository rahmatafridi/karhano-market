using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IStore
    {
        List<StoreModel> GetStores();   
        Guid AddStore(StoreModel model);
        bool UpdateStore(StoreModel store);
        bool DeleteStore(Guid Id);
        StoreModel GetStore(Guid Id);
    }
}
