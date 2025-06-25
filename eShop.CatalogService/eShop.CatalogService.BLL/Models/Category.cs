namespace eShop.CatalogService.BLL.Models;

public class Category : Entity
{
    public required string Name { get; set; }

    public string? Image { get; set; }

    public Category? ParentCategory { get; set; }
}
