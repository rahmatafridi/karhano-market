using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Model
{
    public class SubCategoryVM : CommonFields
    {

        public string Name { get; set; }

        public Guid  CategoryId { get; set; }
        public string Category { get; set; }

        public List<CategoryVM> Categories { get; set; }

        public List<SubCategoryVM> GetSubCategoryByCategoryId { get; set; }

    }
}
