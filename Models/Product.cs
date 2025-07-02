using System;

namespace KarhanoMarket.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public Guid CompanyId { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? SubcategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Property
        public virtual Company Company { get; set; } = null!;
        public virtual Category? Category { get; set; }
        public virtual Subcategory? Subcategory { get; set; }

        // Audit Properties
        public string? CreatedById { get; set; }
        public string? UpdatedById { get; set; }
    }
}
