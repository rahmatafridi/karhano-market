using System;
using System.ComponentModel.DataAnnotations;

namespace BAL.Model
{
    public class UserVM
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Active")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        public Guid RoleId { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Store")]
        public Guid? StoreId { get; set; }

        [Display(Name = "Store Name")]
        public string StoreName { get; set; }

        [Display(Name = "Store Type")]
        public string StoreTypeName { get; set; }

        // Role flags
        public bool IsSuperAdmin { get; set; }
        public bool IsStoreAdmin { get; set; }

        // Impersonation
        public Guid? ImpersonatedByUserId { get; set; }
        public string ImpersonatedByUserName { get; set; }
        public DateTime? LastImpersonationDate { get; set; }

        // Audit fields
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Last Login IP")]
        public string LastLoginIP { get; set; }

        // For password change/reset
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be between 6 and 100 characters")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        // Display properties
        [Display(Name = "Status")]
        public string StatusText => Status ? "Active" : "Inactive";

        [Display(Name = "Role Type")]
        public string RoleType
        {
            get
            {
                if (IsSuperAdmin) return "Super Admin";
                if (IsStoreAdmin) return "Store Admin";
                return "User";
            }
        }

        public bool IsImpersonated => ImpersonatedByUserId.HasValue;
    }
}
