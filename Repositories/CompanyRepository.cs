using KarhanoMarket.Data;
using KarhanoMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace KarhanoMarket.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Company?> GetCompanyWithUsersAsync(int companyId)
        {
            return await _context.Companies
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyWithProductsAsync(int companyId)
        {
            return await _context.Companies
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company?> GetCompanyByUserIdAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !user.CompanyId.HasValue)
                return null;

            return await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == user.CompanyId.Value);
        }

        public async Task<IEnumerable<Company>> GetActiveCompaniesAsync()
        {
            return await _context.Companies
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<bool> IsUserInCompanyAsync(string userId, int companyId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user != null && user.CompanyId == companyId;
        }

        public async Task<IEnumerable<ApplicationUser>> GetCompanyUsersAsync(int companyId)
        {
            return await _context.Users
                .Where(u => u.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetCompanyProductsAsync(int companyId)
        {
            return await _context.Products
                .Where(p => p.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<bool> CanUserAccessCompanyAsync(string userId, int companyId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            if (user.CompanyId == null)
                return true; // SuperAdmin can access all companies

            return user.CompanyId == companyId;
        }

        public async Task<Dictionary<int, int>> GetCompaniesUserCountAsync()
        {
            return await _context.Users
                .Where(u => u.CompanyId != null)
                .GroupBy(u => u.CompanyId!.Value)
                .Select(g => new { CompanyId = g.Key, UserCount = g.Count() })
                .ToDictionaryAsync(x => x.CompanyId, x => x.UserCount);
        }

        public async Task<Dictionary<int, int>> GetCompaniesProductCountAsync()
        {
            return await _context.Products
                .GroupBy(p => p.CompanyId)
                .Select(g => new { CompanyId = g.Key, ProductCount = g.Count() })
                .ToDictionaryAsync(x => x.CompanyId, x => x.ProductCount);
        }
    }
}
