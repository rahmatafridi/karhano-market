using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
  public interface ICategory
    {
        List<CategoryVM> GetCategories();
        Guid AddCategory(CategoryVM category);

        bool UpdateCategory(CategoryVM category);

        bool DeleteCategory(Guid Id);

        CategoryVM GetCategoryById(Guid Id);

    }
}
