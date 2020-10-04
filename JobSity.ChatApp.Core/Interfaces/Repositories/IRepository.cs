using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JobSity.ChatApp.Core.Interfaces.Repositories
{
    public interface IRepository<T> 
        where T : class
    {
         Task<T> GetById(object id);
         Task<IEnumerable<T>> Get(
             Expression<Func<T,bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             string includeProperties = "",
             int limit = 0
         );
         Task Insert(T entity);
         Task Update(T entity);
         Task Delete(object id);
         void Delete(T entity);
         

    }
}