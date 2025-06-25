using eShop.CartService.DAL.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eShop.CartService.DAL;

public class DbContext
{
    public IMongoDatabase Database { get; private set; }

    public DbContext(IOptions<CartsDatabaseSettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);

        Database = mongoClient.GetDatabase(options.Value.DatabaseName);
    }
}
