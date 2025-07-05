# KWebPortal Seed Data

This document contains information about the initial seed data created for the KWebPortal application.

## Default Users

### Super Administrator
- **Email**: `admin@kwebportal.com`
- **Password**: `Admin@123`
- **Role**: SuperAdmin
- **Store**: None (Global access)
- **Permissions**: 
  - Full system access
  - Can manage all stores and users
  - Can impersonate any user
  - Can create/edit/delete stores and store types

### Store Administrators

#### Store Admin 1 (Main Retail Store)
- **Email**: `storeadmin1@kwebportal.com`
- **Password**: `StoreAdmin@123`
- **Role**: StoreAdmin
- **Store**: Main Retail Store
- **Permissions**:
  - Manage users within their store
  - Manage products for their store
  - Cannot impersonate users
  - Store-specific access only

#### Store Admin 2 (Online Store)
- **Email**: `storeadmin2@kwebportal.com`
- **Password**: `StoreAdmin@123`
- **Role**: StoreAdmin
- **Store**: Online Store
- **Permissions**:
  - Manage users within their store
  - Manage products for their store
  - Cannot impersonate users
  - Store-specific access only

## Default Roles

1. **SuperAdmin** - Full system access
2. **StoreAdmin** - Store-specific administrative access
3. **User** - Regular user access

## Default Store Types

1. **Retail Store** - Physical retail locations
2. **Online Store** - E-commerce stores

## Default Stores

1. **Main Retail Store** (Retail Store type)
2. **Online Store** (Online Store type)

## Default Categories

1. **Electronics**
2. **Clothing**

## Database Migration

To apply the seed data to your database:

1. **Add Migration** (if not already done):
   ```bash
   dotnet ef migrations add SeedData --project DAL --startup-project KWebPortal
   ```

2. **Update Database**:
   ```bash
   dotnet ef database update --project DAL --startup-project KWebPortal
   ```

## Security Notes

- All passwords are hashed using BCrypt with work factor 12
- The seed data uses fixed GUIDs for consistency across environments
- Change default passwords in production environments
- The super admin account should be secured with a strong password

## Testing the Application

1. **Login as Super Admin**:
   - Navigate to `/Account/Login`
   - Use credentials: `admin@kwebportal.com` / `Admin@123`
   - You should have access to all features including user management and impersonation

2. **Test Impersonation**:
   - As super admin, go to User Management
   - Click the impersonation button for any store admin
   - Verify the impersonation bar appears
   - Test store-specific functionality
   - Use "Stop Impersonating" to return to super admin

3. **Login as Store Admin**:
   - Use either store admin account
   - Verify you only see store-specific data and features
   - Confirm you cannot access impersonation features

## Customization

To modify the seed data:

1. Edit `DAL/SeedData/DbSeeder.cs`
2. Update the GUIDs, names, emails, or other properties as needed
3. Generate a new migration
4. Update the database

## Production Deployment

Before deploying to production:

1. Change all default passwords
2. Remove or secure test accounts
3. Ensure proper SSL/TLS configuration
4. Review and update security settings
5. Configure proper logging and monitoring
