namespace eShop.CatalogService.BLL.Models;

public class Product : Entity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int CategoryId { get; set; }

    public required Category Category { get; set; }

    public required decimal Price { get; set; }

    public int Amount { get; set; }
}
