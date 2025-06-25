using eShop.CartService.BLL.Models;

namespace eShop.CartService.BLL.Interfaces;

public interface ICartsRepository
{
    Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId);

    Task AddCartItemAsync(CartItem item, string cartId);

    Task UpdateCartItemAsync(CartItem item);

    Task<bool> RemoveCartItemAsync(int itemId, string cartId);
}
