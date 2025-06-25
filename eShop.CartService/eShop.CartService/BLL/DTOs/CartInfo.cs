namespace eShop.CartService.BLL.DTOs;

public class CartInfo
{
    public string Id { get; set; }

    public IEnumerable<CartItemInfo> Items { get; set; }
}
