using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Models;

namespace eShop.CatalogService.BLL.Services;

public class ProductService : BaseCrudService<Product>
{
    private const int MinNameLength = 3;
    private const int MaxNameLength = 50;

    public ProductService(IRepository<Product> repository) : base(repository)
    {
    }

    protected override void ValidateEntity(Product entity)
    {
        if (entity.Name.Length < MinNameLength || entity.Name.Length > MaxNameLength)
        {
            throw new ArgumentException($"Min name length should be from {MinNameLength} to {MaxNameLength}");
        }

        if (entity.Image != null && !Utils.IsValidUrl(entity.Image))
        {
            throw new ArgumentException($"Image should be valid url");
        }
    }
}
