using PRN231.Models;
using PRN231.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PRN231.DAL;
using PRN231.Repository.Interfaces;

namespace PRN231.Repository.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SmartHeadContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SmartHeadContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(params int[] keys)
        {
            var entity = await Get(keys);
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll(params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            var list = await query.ToListAsync();
            return list;
        }

        public async Task<T> Get(params int[] keys)
        {
            if (keys.Length == 1)
            {
                var entity1 = await _dbSet.FindAsync(keys[0]);
                return entity1;
            }
            var entity = _dbSet.Find(keys[0], keys[1]);
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
