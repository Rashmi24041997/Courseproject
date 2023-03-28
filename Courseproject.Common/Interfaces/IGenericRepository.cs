using Courseproject.Common.Model;
using System.Linq.Expressions;

namespace Courseproject.Common.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>>[] filters, int? skip, int? take, params Expression<Func<T, object>>[] includes);
    Task<List<T>> GetAsync(int? skip, int? take, params Expression<Func<T, object>>[] includes);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<int> InsertAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}
