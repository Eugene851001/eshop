namespace eShop.CatalogService.BLL.Models;

public class ProductsFilterModel
{
    public int? CategoryId { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 50;
}
