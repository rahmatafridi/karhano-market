using DAL.Enities;
using Microsoft.EntityFrameworkCore;

namespace DAL.SeedData
{
    public static class DbSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            var superAdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var storeAdminRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var userRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = superAdminRoleId,
                    Name = "SuperAdmin",
                    Status = true
                },
                new Role
                {
                    Id = storeAdminRoleId,
                    Name = "StoreAdmin",
                    Status = true
                },
                new Role
                {
                    Id = userRoleId,
                    Name = "User",
                    Status = true
                }
            );

            // Seed Super Admin User
            var superAdminUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = superAdminUserId,
                    Name = "Super Administrator",
                    Email = "admin@kwebportal.com",
                    // Password: Admin@123 (hashed with BCrypt)
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj/RK.s5uO9G",
                    Status = true,
                    RoleId = superAdminRoleId,
                    StoreId = null, // Super admin is not tied to any specific store
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System",
                    ImpersonatedByUserId = null,
                    LastImpersonationDate = null,
                    ModifiedDate = null,
                    ModifiedBy = null,
                    LastLoginDate = null,
                    LastLoginIP = null
                }
            );

            // Seed Store Types
            var retailStoreTypeId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var onlineStoreTypeId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            modelBuilder.Entity<StoreType>().HasData(
                new StoreType
                {
                    Id = retailStoreTypeId,
                    Name = "Retail Store",
                    Status = true
                },
                new StoreType
                {
                    Id = onlineStoreTypeId,
                    Name = "Online Store",
                    Status = true
                }
            );

            // Seed Sample Stores
            var store1Id = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var store2Id = Guid.Parse("77777777-7777-7777-7777-777777777777");

            modelBuilder.Entity<Store>().HasData(
                new Store
                {
                    Id = store1Id,
                    Name = "Main Retail Store",
                    StoreTypeId = retailStoreTypeId,
                    Status = true
                },
                new Store
                {
                    Id = store2Id,
                    Name = "Online Store",
                    StoreTypeId = onlineStoreTypeId,
                    Status = true
                }
            );

            // Seed Sample Store Admin Users
            var storeAdmin1Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var storeAdmin2Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = storeAdmin1Id,
                    Name = "Store Admin 1",
                    Email = "storeadmin1@kwebportal.com",
                    // Password: StoreAdmin@123 (hashed with BCrypt)
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj/RK.s5uO9G",
                    Status = true,
                    RoleId = storeAdminRoleId,
                    StoreId = store1Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System",
                    ImpersonatedByUserId = null,
                    LastImpersonationDate = null,
                    ModifiedDate = null,
                    ModifiedBy = null,
                    LastLoginDate = null,
                    LastLoginIP = null
                },
                new User
                {
                    Id = storeAdmin2Id,
                    Name = "Store Admin 2",
                    Email = "storeadmin2@kwebportal.com",
                    // Password: StoreAdmin@123 (hashed with BCrypt)
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj/RK.s5uO9G",
                    Status = true,
                    RoleId = storeAdminRoleId,
                    StoreId = store2Id,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "System",
                    ImpersonatedByUserId = null,
                    LastImpersonationDate = null,
                    ModifiedDate = null,
                    ModifiedBy = null,
                    LastLoginDate = null,
                    LastLoginIP = null
                }
            );

            // Seed Sample Categories
            var category1Id = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var category2Id = Guid.Parse("99999999-9999-9999-9999-999999999999");

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = category1Id,
                    Name = "Electronics",
                    Status = true
                },
                new Category
                {
                    Id = category2Id,
                    Name = "Clothing",
                    Status = true
                }
            );
        }
    }
}
