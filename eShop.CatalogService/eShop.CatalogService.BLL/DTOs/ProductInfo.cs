namespace eShop.CatalogService.BLL.DTOs;

public class ProductInfo
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public int CategoryId { get; set; } = -1;

    public decimal Price { get; set; }

    public int Amount { get; set; }
}
