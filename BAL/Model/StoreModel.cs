using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Model
{
    public class StoreModel :CommonFields
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public Guid TypeId { get; set; }
        public string StoreType { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string OwnerName { get; set; }
        public List<StoreTypeModel> StoreTypes { get; set; }    
    }
}
