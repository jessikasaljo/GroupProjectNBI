using Domain.RepositoryInterface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken); // Pass the cancellationToken to FindAsync
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken); // Pass the cancellationToken to ToListAsync
        }
        public async Task<IEnumerable<T>> GetPageAsync(int skip, int take, CancellationToken cancellationToken)
        {
            return await _dbSet.Skip(skip).Take(take).ToListAsync(cancellationToken); // Pass the cancellationToken to ToListAsync
        }

        public async Task AddAsync(T item, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(item, cancellationToken); // Pass the cancellationToken to AddAsync
            await _context.SaveChangesAsync(cancellationToken); // Pass the cancellationToken to SaveChangesAsync
        }

        public async Task UpdateAsync(T item, CancellationToken cancellationToken)
        {
            _dbSet.Update(item); // Update does not have an async version that accepts CancellationToken, so you don't need to pass it here
            await _context.SaveChangesAsync(cancellationToken); // Pass the cancellationToken to SaveChangesAsync
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken); // Pass cancellationToken to GetByIdAsync
            if (entity != null)
            {
                _dbSet.Remove(entity); // Remove the entity
                await _context.SaveChangesAsync(cancellationToken); // Pass the cancellationToken to SaveChangesAsync
            }
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken); // Pass the cancellationToken to FirstOrDefaultAsync
        }
    }

}
