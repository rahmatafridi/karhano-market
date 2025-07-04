using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface ISubCategory
    {
        List<SubCategoryVM> GetSubCategory();
        List<SubCategoryVM> GetByCategoryId(Guid Id);
        Guid AddSubCategory(SubCategoryVM model);
        bool UpdateSubCategory(SubCategoryVM model);
        bool DeleteSubCategory(Guid Id);
        SubCategoryVM GetSubCategory(Guid Id);

        List<SubCategoryVM> GetSubCategoryByCategoryId(Guid Id);
    }
}
