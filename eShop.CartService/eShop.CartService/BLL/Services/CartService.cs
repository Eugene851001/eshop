using eShop.CartService.BLL.Interfaces;
using eShop.CartService.BLL.Models;

namespace eShop.CartService.BLL.Services;

public class CartService
{
    private readonly ICartsRepository _cartRepository;

    public CartService(ICartsRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public Task<IEnumerable<CartItem>> GetCartItemsAsync(string cartId)
    {
        ArgumentException.ThrowIfNullOrEmpty(cartId, nameof(cartId));

        return _cartRepository.GetCartItemsAsync(cartId);
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        await _cartRepository.UpdateCartItemAsync(cartItem);
    }

    public Task AddCartItemAsync(CartItem cartItem, string cartId)
    {
        ArgumentException.ThrowIfNullOrEmpty(cartId, nameof(cartId));

        if (cartItem.ImageUrl != null &&
            !Uri.TryCreate(cartItem.ImageUrl, new UriCreationOptions(), out var _))
        {
            throw new ArgumentException("Invalid URL format", nameof(cartItem.ImageUrl));
        }

        return _cartRepository.AddCartItemAsync(cartItem, cartId);
    }

    public Task<bool> DeleteCartItemAsync(int itemId, string cartId)
    {
        ArgumentException.ThrowIfNullOrEmpty(cartId, nameof(cartId));

        return _cartRepository.RemoveCartItemAsync(itemId, cartId);
    }
}
