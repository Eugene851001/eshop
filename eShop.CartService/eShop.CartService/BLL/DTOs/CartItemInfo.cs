namespace eShop.CartService.BLL.DTOs;

public class CartItemInfo
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageText { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}
