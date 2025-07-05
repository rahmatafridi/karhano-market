using DAL.Enities;
using DAL.SeedData;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class KWebContext : DbContext
    {
        public KWebContext(DbContextOptions<KWebContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired();

                // User - Role relationship
                entity.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                // User - Store relationship
                entity.HasOne(e => e.Store)
                    .WithMany()
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.SetNull);

                // User - Impersonation relationship (self-referencing)
                entity.HasOne(e => e.ImpersonatedByUser)
                    .WithMany(e => e.ImpersonatedUsers)
                    .HasForeignKey(e => e.ImpersonatedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Audit fields
                entity.Property(e => e.CreatedDate)
                    .IsRequired();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100);

                entity.Property(e => e.LastLoginIP)
                    .HasMaxLength(50);

                // Indexes
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.StoreId);
                entity.HasIndex(e => e.RoleId);
            });

            // Role configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Store configuration
            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(e => e.StoreType)
                    .WithMany()
                    .HasForeignKey(e => e.StoreTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Store)
                    .WithMany()
                    .HasForeignKey(e => e.StoreId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubCategory)
                    .WithMany()
                    .HasForeignKey(e => e.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            // SubCategory configuration
            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial data
            DbSeeder.SeedData(modelBuilder);
        }
    }
}
