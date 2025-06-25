using System.Linq.Expressions;

using eShop.CatalogService.BLL.Exceptions;
using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Models;

namespace eShop.CatalogService.BLL.Services;

public abstract class BaseCrudService<T> where T : Entity
{
    protected IRepository<T> Repository { get; set; }

    public BaseCrudService(IRepository<T> repository)
    {
        Repository = repository;
    }

    public virtual async Task AddAsync(T entity)
    {
        ValidateEntity(entity);

        Repository.Add(entity);
        await Repository.SaveAsync();
    }

    public async virtual Task<int> DeleteManyAsync(Expression<Func<T, bool>> predicate)
    {
        return await Repository.DeleteManyAsync(predicate);
    }

    public async virtual Task<bool> DeleteAsync(int id)
    {
        Repository.Delete(id);
        return await Repository.SaveAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        ValidateEntity(entity);

        Repository.Update(entity);
        try
        {
            await Repository.SaveAsync();
        }
        catch (Exception ex) when (ex.Message.Contains("affected 0 row(s)"))
        {
            throw new EntityNotFoundException($"Entity with id {entity.Id} not found");
        }
    }

    public async virtual Task<T> GetSingle(int id)
    {
        return await Repository.GetAsync(id);
    }

    public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, int? skip = null, int? take = null)
    {
        return await Repository.GetAllAsync(filter, skip, take);
    }

    protected abstract void ValidateEntity(T entity);
}
