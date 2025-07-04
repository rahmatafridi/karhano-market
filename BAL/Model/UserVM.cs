using System;
using System.ComponentModel.DataAnnotations;

namespace BAL.Model
{
    public class UserVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public Guid? StoreId { get; set; }
        public string StoreName { get; set; }

        public bool IsSuperAdmin { get; set; }
        public bool IsStoreAdmin { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // For impersonation
        public Guid? ImpersonatedByUserId { get; set; }
        public Guid? OriginalUserId { get; set; }
    }
}
