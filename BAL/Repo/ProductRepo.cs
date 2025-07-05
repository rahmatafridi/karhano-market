using BAL.Interfaces;
using BAL.Model;
using DAL;
using DAL.Enities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Repo
{
    public class ProductRepo : IProduct
    {
        private readonly KWebContext _context;

        public ProductRepo(KWebContext context)
        {
            _context = context;
        }

        public Guid AddProduct(ProductVM model)
        {
            var product = new Product
            {
                Id = model.Id,
                Name = model.Name,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                Status = model.Status,
                Description = "",
                CategoryId = model.CategoryId,
                PurchasePrice = model.PurchasePrice,
                SalePrice = model.SalePrice,
                CrossPrice = model.CrossPrice,
                Disscount = model.Disscount,
                StoreId = model.StoreId,
                SubCategoryId = model.SubCategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return product.Id;
        }

        public bool DeleteProduct(Guid id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public ProductVM GetProduct(Guid id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
                return null;

            return new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                UpdatedBy = product.UpdatedBy,
                UpdatedDate = product.UpdatedDate,
                CreatedDate = product.CreatedDate,
                CreatedBy = (Guid)product.CreatedBy,
                Status = product.Status,
                Description = product.Description,
                CategoryId = product.CategoryId,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                CrossPrice = product.CrossPrice,
                Disscount = product.Disscount,
                StoreId = product.StoreId,
                SubCategoryId = product.SubCategoryId
            };
        }

        public List<ProductVM> GetProducts(Guid id)
        {
            var model = new List<ProductVM>();
            var products = _context.Products.Where(x=>x.StoreId ==id).ToList();

            foreach (var item in products)
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == item.CategoryId);
                var store = _context.Stores.FirstOrDefault(x => x.Id == item.StoreId);
                var subcategory = _context.SubCategories.FirstOrDefault(x => x.Id == item.SubCategoryId);

                model.Add(new ProductVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Status = item.Status,
                    CreatedDate = item.CreatedDate,
                    CreatedBy = (Guid)item.CreatedBy,
                    UpdatedBy = item.UpdatedBy,
                    SalePrice = item.SalePrice,
                    CrossPrice = item.CrossPrice,
                    Disscount = item.Disscount,
                    StoreId = item.StoreId,
                    PurchasePrice = item.PurchasePrice,
                    SubCategoryId = item.SubCategoryId,
                    Description = item.Description,
                    Store = store?.Name ?? "",
                    Category = category?.Name ?? "",
                    SubCategory = subcategory?.Name ?? ""
                });
            }

            return model;
        }

        public bool UpdateProduct(ProductVM product)
        {
            var model = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (model != null)
            {
                model.Name = product.Name;
                model.UpdatedBy = product.UpdatedBy;
                model.UpdatedDate = product.UpdatedDate;
                model.CreatedDate = product.CreatedDate;
                model.CreatedBy = product.CreatedBy;
                model.Status = product.Status;
                model.Description = product.Description;
                model.CategoryId = product.CategoryId;
                model.PurchasePrice = product.PurchasePrice;
                model.SalePrice = product.SalePrice;
                model.CrossPrice = product.CrossPrice;
                model.Disscount = product.Disscount;
                model.StoreId = product.StoreId;
                model.SubCategoryId = product.SubCategoryId;

                _context.Products.Update(model);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<ProductImageVM> GetImagesByProdutId(Guid id)
        {
            return _context.ProductImages
                .Where(x => x.ProductId == id)
                .Select(img => new ProductImageVM
                {
                    Id = img.Id,
                    Name = img.Name,
                    FilePath = img.FilePath,
                    ProductId = img.ProductId
                })
                .ToList();
        }

        public bool UploadImages(List<ProductImageVM> imageFiles)
        {
            if (imageFiles == null || !imageFiles.Any())
                return false;

            foreach (var image in imageFiles)
            {
                var entity = new ProductImage
                {
                    Id = image.Id,
                    Name = image.Name,
                    FilePath = image.FilePath,
                    ProductId = image.ProductId
                };

                _context.ProductImages.Add(entity);
            }

            _context.SaveChanges();
            return true;
        }
    }
}
