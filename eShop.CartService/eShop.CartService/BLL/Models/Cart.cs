using System.Collections.ObjectModel;

namespace eShop.CartService.BLL.Models;

public class Cart
{
    public required string Id { get; set; }

    public Collection<CartItem> Items { get; set; } = new Collection<CartItem>();
}
