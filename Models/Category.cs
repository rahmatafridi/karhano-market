using System;
using System.Collections.Generic;

namespace KarhanoMarket.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
