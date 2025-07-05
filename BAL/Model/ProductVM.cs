using DAL.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BAL.Model
{
   public class ProductVM : CommonFields
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

        public Guid CreatedBy { get; set; }

        public string Store { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public List<StoreModel> Stores { get; set; }
        public List<CategoryVM> Categories { get; set; }
        public List<SubCategoryVM> SubCategories { get; set; }

        public List<IFormFile>? Images { get; set; }

    }
}
