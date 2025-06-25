using System.Linq.Expressions;

namespace eShop.CatalogService.BLL.Interfaces;

public interface IRepository<T>
{
    Task<T> GetAsync(int id);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, int? skip = null, int? take = null);

    void Update(T entity);

    void Add(T entity);

    void Delete(int id);

    Task<int> DeleteManyAsync(Expression<Func<T, bool>> filter);

    Task<bool> SaveAsync();
}
