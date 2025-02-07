using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderApi.Repositories.Contracts
{
    public interface IRepository<T>
    {
        Task<IList<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T?> Search(Expression<Func<T, bool>> predicate);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}