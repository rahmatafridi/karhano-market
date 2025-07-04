using BAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BAL.Interfaces
{
   public interface IProduct
    {
        List<ProductVM> GetProducts();

        public List<ProductImageVM> GetImagesByProdutId(Guid Id);

        public bool UploadImages(List<ProductImageVM> imageFiles);
        Guid AddProduct(ProductVM product);

        bool UpdateProduct(ProductVM product);

        bool DeleteProduct(Guid Id);

        ProductVM GetProduct(Guid Id);
    }
}
