using KarhanoMarket.Data;
using KarhanoMarket.Models;
using Microsoft.Extensions.Logging;

namespace KarhanoMarket.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context, ILogger<GenericRepository<Image>> logger) : base(context, logger)
        {
            _context = context;
        }
    }
}
