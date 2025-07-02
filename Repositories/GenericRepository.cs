using KarhanoMarket.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KarhanoMarket.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(
            ApplicationDbContext context,
            ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> GetByIdAsync(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting {typeof(T).Name} by id: {id}");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting all {typeof(T).Name}s");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error finding {typeof(T).Name}s with predicate");
                throw;
            }
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting first {typeof(T).Name} with predicate");
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding range of {typeof(T).Name}s");
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating {typeof(T).Name}");
                throw;
            }
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            try
            {
                _dbSet.UpdateRange(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating range of {typeof(T).Name}s");
                throw;
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting {typeof(T).Name}");
                throw;
            }
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting range of {typeof(T).Name}s");
                throw;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                return predicate == null 
                    ? await _dbSet.CountAsync() 
                    : await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error counting {typeof(T).Name}s");
                throw;
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking any {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving changes for {typeof(T).Name}");
                throw;
            }
        }
    }
}
