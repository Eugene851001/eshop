using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.CartService.DAL.Models;

public class CartItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int OriginalId { get; set; }

    public required string CartId { get; set; }

    public required string Name { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageText { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}
