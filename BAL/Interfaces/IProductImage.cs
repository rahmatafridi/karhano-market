using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
    public interface IProductImage
    {
        ProductImageVM GetImagesbyId(Guid Id);
        
    }
}
