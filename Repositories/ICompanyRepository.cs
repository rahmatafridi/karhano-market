using KarhanoMarket.Models;

namespace KarhanoMarket.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<Company?> GetCompanyWithUsersAsync(int companyId);
        Task<Company?> GetCompanyWithProductsAsync(int companyId);
        Task<Company?> GetCompanyByUserIdAsync(string userId);
        Task<IEnumerable<Company>> GetActiveCompaniesAsync();
        Task<bool> IsUserInCompanyAsync(string userId, int companyId);
        Task<IEnumerable<ApplicationUser>> GetCompanyUsersAsync(int companyId);
        Task<IEnumerable<Product>> GetCompanyProductsAsync(int companyId);
        Task<bool> CanUserAccessCompanyAsync(string userId, int companyId);
        Task<Dictionary<int, int>> GetCompaniesUserCountAsync();
        Task<Dictionary<int, int>> GetCompaniesProductCountAsync();
    }
}
