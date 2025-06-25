using AutoMapper;
using eShop.CartService.BLL.Interfaces;
using eShop.CartService.DAL.Models;
using MongoDB.Driver;

namespace eShop.CartService.DAL.Repositories;

public class CartsRepository : ICartsRepository
{
    private readonly IMongoCollection<Models.CartItem> _cartItems;
    private readonly IMapper _mapper;

    public CartsRepository(DbContext context, IMapper mapper)
    {
        _cartItems = context.Database.GetCollection<CartItem>("Carts");

        _mapper = mapper;
    }

    public async Task AddCartItemAsync(BLL.Models.CartItem item, string cartId)
    {
        var dbItem = _mapper.Map<CartItem>(item);
        dbItem.CartId = cartId;

        await _cartItems.InsertOneAsync(dbItem);
    }

    public async Task<IEnumerable<BLL.Models.CartItem>> GetCartItemsAsync(string cartId)
    {
        var items = await _cartItems.FindAsync(x => x.CartId == cartId);
        return items.ToList().Select(x => _mapper.Map<BLL.Models.CartItem>(x));
    }

    public async Task<bool> RemoveCartItemAsync(int itemId, string cartId)
    {
        var result = await _cartItems.DeleteOneAsync(x => x.OriginalId == itemId && x.CartId == cartId);

        return result.DeletedCount > 0;
    }

    public async Task UpdateCartItemAsync(BLL.Models.CartItem item)
    {
        var update = Builders<CartItem>.Update
            .Set(x => x.Name, item.Name)
            .Set(x => x.Price, item.Price)
            .Set(x => x.Quantity, item.Quantity);

        await _cartItems.UpdateOneAsync(x => x.OriginalId == item.Id, update);
    }
}
