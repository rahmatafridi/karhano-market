using DAL.Enities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.SeedData
{
    public static class DbSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Role IDs
            var superAdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var storeAdminRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var userRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Store Type IDs
            var retailStoreTypeId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var onlineStoreTypeId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            // Store IDs
            var store1Id = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var store2Id = Guid.Parse("77777777-7777-7777-7777-777777777777");

            // Category IDs
            var category1Id = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var category2Id = Guid.Parse("99999999-9999-9999-9999-999999999999");

            // User IDs
            var superAdminUserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var storeAdmin1Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var storeAdmin2Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

            // 1. Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = superAdminRoleId, Name = "SuperAdmin", Status = true },
                new Role { Id = storeAdminRoleId, Name = "StoreAdmin", Status = true },
                new Role { Id = userRoleId, Name = "User", Status = true }
            );

            // 2. Store Types
            modelBuilder.Entity<StoreType>().HasData(
                new StoreType { Id = retailStoreTypeId, Name = "Retail Store", Status = true },
                new StoreType { Id = onlineStoreTypeId, Name = "Online Store", Status = true }
            );

            // 3. Stores
            modelBuilder.Entity<Store>().HasData(
                new Store
                {
                    Id = store1Id,
                    Name = "Main Retail Store",
                    Description = "Our flagship retail store location",
                    Address = "123 Main Street, Downtown, City 12345",
                    TypeId = retailStoreTypeId,
                    Email = "mainstore@kwebportal.com",
                    ContactNo = "+1-555-0123",
                    ContactPerson = "John Manager",
                    OwnerName = "KWebPortal Corp",
                    Status = true,
                    CreatedDate = new DateTime(2024, 01, 01),
                    CreatedBy = superAdminUserId
                },
                new Store
                {
                    Id = store2Id,
                    Name = "Online Store",
                    Description = "Our e-commerce platform",
                    Address = "456 Digital Avenue, Online Plaza, Virtual 67890",
                    TypeId = onlineStoreTypeId,
                    Email = "onlinestore@kwebportal.com",
                    ContactNo = "+1-555-0456",
                    ContactPerson = "Jane Digital",
                    OwnerName = "KWebPortal Corp",
                    Status = true,
                    CreatedDate = new DateTime(2024, 01, 01),
                    CreatedBy = superAdminUserId
                }
            );

            // 4. Users (after stores!)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = superAdminUserId,
                    Name = "Super Administrator",
                    Email = "admin@kwebportal.com",
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj/RK.s5uO9G",
                    Status = true,
                    RoleId = superAdminRoleId,
                    StoreId = null,
                    CreatedDate = new DateTime(2024, 01, 01),
                    CreatedBy = "System",
                    ImpersonatedByUserId = null,
                    LastImpersonationDate = new DateTime(2024, 01, 01),
                    ModifiedDate = new DateTime(2024, 01, 01),
                    ModifiedBy = "System",
                    LastLoginDate = new DateTime(2024, 01, 01),
                    LastLoginIP = "127.0.0.1"
                },
                new User
                {
                    Id = storeAdmin1Id,
                    Name = "Store Admin 1",
                    Email = "storeadmin1@kwebportal.com",
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj/RK.s5uO9G",
                    Status = true,
                    RoleId = storeAdminRoleId,
                    StoreId = store1Id,
                    CreatedDate = new DateTime(2024, 01, 01),
                    CreatedBy = "System",
                    ImpersonatedByUserId = null,
                    LastImpersonationDate = new DateTime(2024, 01, 01),
                    ModifiedDate = new DateTime(2024, 01, 01),
                    ModifiedBy = "System",
                    LastLoginDate = new DateTime(2024, 01, 01),
                    LastLoginIP = "127.0.0.1"
                },
                new User
                {
                    Id = storeAdmin2Id,
                    Name = "Store Admin 2",
                    Email = "storeadmin2@kwebportal.com",
                    Password = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj/RK.s5uO9G",
                    Status = true,
                    RoleId = storeAdminRoleId,
                    StoreId = store2Id,
                    CreatedDate = new DateTime(2024, 01, 01),
                    CreatedBy = "System",
                    ImpersonatedByUserId = null,
                    LastImpersonationDate = new DateTime(2024, 01, 01),
                    ModifiedDate = new DateTime(2024, 01, 01),
                    ModifiedBy = "System",
                    LastLoginDate = new DateTime(2024, 01, 01),
                    LastLoginIP = "127.0.0.1"
                }
            );

            // 5. Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = category1Id, Name = "Electronics", Status = true },
                new Category { Id = category2Id, Name = "Clothing", Status = true }
            );
        }
    }
}
