using Microsoft.AspNetCore.Identity;
using System;

namespace KarhanoMarket.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;

        public Guid? CompanyId { get; set; }

        public virtual Company? Company { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        [NonSerialized]
        public bool IsImpersonated { get; set; }

        public Guid? ImpersonatorId { get; set; }
    }
}
