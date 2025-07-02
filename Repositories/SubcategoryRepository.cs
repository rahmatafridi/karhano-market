using KarhanoMarket.Data;
using KarhanoMarket.Models;
using Microsoft.Extensions.Logging;

namespace KarhanoMarket.Repositories
{
    public class SubcategoryRepository : GenericRepository<Subcategory>, ISubcategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public SubcategoryRepository(ApplicationDbContext context, ILogger<GenericRepository<Subcategory>> logger) : base(context, logger)
        {
            _context = context;
        }
    }
}
