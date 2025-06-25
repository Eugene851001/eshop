namespace eShop.CatalogService.BLL.Models;

public class ProductUpdatedMessage
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}
