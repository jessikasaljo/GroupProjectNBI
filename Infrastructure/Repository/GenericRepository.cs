﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.RepositoryInterface;
using Infrastructure.Database;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly RealDatabase _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(RealDatabase context)
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
