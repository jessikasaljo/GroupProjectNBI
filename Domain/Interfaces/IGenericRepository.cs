

using System.Linq.Expressions;

namespace Domain.RepositoryInterface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetPageAsync(int skip, int size, CancellationToken cancellationToken);
        Task AddAsync(T item, CancellationToken cancellationToken);
        Task UpdateAsync(T item, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }
}
