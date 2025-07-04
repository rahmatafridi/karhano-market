using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Model
{
  public  class CategoryVM : CommonFields
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool Status { get; set; }

        public List<SubCategoryVM> SubCategoryList { get; set; }
    }
}
