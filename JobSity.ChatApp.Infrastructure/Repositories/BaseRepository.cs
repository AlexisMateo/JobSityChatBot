using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JobSity.ChatApp.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobSity.ChatApp.Infrastructure.Repositories
{
    public class BaseRepository<T, TContext> : IRepository<T> 
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int limit = 0)
        {
            IQueryable<T> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach(var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include<T>(includeProperty);
                }
            }

            if(orderBy != null)
            {
                query = orderBy(query);
            }

            if(limit > 0)
            {
                return await query.Take(limit).ToListAsync();
            }
            
            return await query.ToListAsync();
        }

        public async Task<T> Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }


        public async Task Delete(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            Delete(entity);
        }

        public void Delete(T entity)
        {
            if(_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbContext.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

    }
}