using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Enities
{
   public class Product : CommonFields
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public Guid CategoryId { get; set; }

        public Guid SubCategoryId { get; set; }

        public Decimal PurchasePrice { get; set; }

        public Decimal SalePrice { get; set; }

        public Decimal CrossPrice { get; set; }

        public Decimal Disscount { get; set; }

        public Guid StoreId { get; set; }



    }
}
