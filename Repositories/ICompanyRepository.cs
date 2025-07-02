using KarhanoMarket.Models;

namespace KarhanoMarket.Repositories
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<Company?> GetCompanyWithUsersAsync(Guid companyId);
        Task<Company?> GetCompanyWithProductsAsync(Guid companyId);
        Task<Company?> GetCompanyByUserIdAsync(Guid userId);
        Task<IEnumerable<Company>> GetActiveCompaniesAsync();
        Task<bool> IsUserInCompanyAsync(Guid userId, Guid companyId);
        Task<IEnumerable<ApplicationUser>> GetCompanyUsersAsync(Guid companyId);
        Task<IEnumerable<Product>> GetCompanyProductsAsync(Guid companyId);
        Task<bool> CanUserAccessCompanyAsync(Guid userId, Guid companyId);
        Task<Dictionary<Guid, int>> GetCompaniesUserCountAsync();
        Task<Dictionary<Guid, int>> GetCompaniesProductCountAsync();
    }
}
