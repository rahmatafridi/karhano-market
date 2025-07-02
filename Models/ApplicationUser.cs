using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KarhanoMarket.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Null indicates SuperAdmin; non-null links to a specific company/store
        public int? CompanyId { get; set; }

        // Navigation property
        public virtual Company? Company { get; set; }

        // Additional properties for user management
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Property to check if user is being impersonated
        [NonSerialized]
        public bool IsImpersonated { get; set; }

        // Property to store the ID of the admin doing the impersonation
        public string? ImpersonatorId { get; set; }
    }
}
