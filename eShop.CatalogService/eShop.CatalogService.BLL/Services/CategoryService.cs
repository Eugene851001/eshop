using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Models;

namespace eShop.CatalogService.BLL.Services;

public class CategoryService : BaseCrudService<Category>
{
    private const int MinCatalogName = 3;
    private const int MaxCatalogName = 50;

    public CategoryService(IRepository<Category> repository) : base(repository)
    {
    }

    protected override void ValidateEntity(Category entity)
    {
        if (entity.Name.Length < MinCatalogName || entity.Name.Length > MaxCatalogName)
        {
            throw new ArgumentException($"Min name length should be from {MinCatalogName} to {MaxCatalogName}");
        }

        if (entity.Image != null && !Utils.IsValidUrl(entity.Image))
        {
            throw new ArgumentException($"Image should be valid url");
        }
    }
}
