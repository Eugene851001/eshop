using System.Linq.Expressions;

using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Models;
using eShop.CatalogService.DAL.Contexts;

using Microsoft.EntityFrameworkCore;

namespace eShop.CatalogService.DAL.Repositories;

public class GenericRepository<T> : IRepository<T> where T : Entity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _table;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        _table = dbContext.Set<T>();
    }

    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void Delete(int id)
    {
        var entity = _context.Set<T>().Find(id);

        if (entity != null)
        {
            entity.Deleted = true;
        }
    }

    public async Task<int> DeleteManyAsync(Expression<Func<T, bool>> filter)
    {
        return await _table.Where(filter).ExecuteUpdateAsync(s => s.SetProperty(p => p.Deleted, true));
    }

    public async Task<T> GetAsync(int id)
    {
        return await _table.Where(x => x.Deleted == false && x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, int? skip = null, int? take = null)
    {
        IQueryable<T> result = _table;

        if (filter != null)
        {
            result = result.Where(filter);
        }

        if (skip != null)
        {
            result = result.Skip(skip.Value);
        }

        if (take != null)
        {
            result = result.Take(take.Value);
        }

        return await result.Where(x => x.Deleted == false).ToListAsync();
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<bool> SaveAsync()
    {
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
