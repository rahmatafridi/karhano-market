using KarhanoMarket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KarhanoMarket.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure global query filters for multi-tenancy
            builder.Entity<Product>()
                .HasQueryFilter(p => !GetCurrentUserCompanyId().HasValue || p.CompanyId == GetCurrentUserCompanyId());

            // Configure relationships
            builder.Entity<Company>()
                .HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Company>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add indexes
            builder.Entity<Product>()
                .HasIndex(p => p.CompanyId);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.CompanyId);
        }

        private Guid? GetCurrentUserCompanyId()
        {
            // Get the current user from HttpContext
            var user = _httpContextAccessor.HttpContext?.User;

            // If user is a SuperAdmin (has SuperAdmin role), return null to see all data
            if (user?.IsInRole("SuperAdmin") ?? false)
                return null;

            // Otherwise, try to get CompanyId from claims
            var companyIdClaim = user?.FindFirst("CompanyId");
            return companyIdClaim != null ? Guid.Parse(companyIdClaim.Value) : (Guid?)null;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set UpdatedAt when entities are modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Product && e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Product product)
                {
                    product.UpdatedAt = DateTime.UtcNow;

                    // Set UpdatedById if we can get the current user
                    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        product.UpdatedById = userId;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
