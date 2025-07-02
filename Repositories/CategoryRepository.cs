using KarhanoMarket.Data;
using KarhanoMarket.Models;
using Microsoft.Extensions.Logging;

namespace KarhanoMarket.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context, ILogger<GenericRepository<Category>> logger) : base(context, logger)
        {
            _context = context;
        }
    }
}
