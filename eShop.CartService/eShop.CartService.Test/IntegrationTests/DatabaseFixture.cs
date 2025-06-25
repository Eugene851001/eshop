using eShop.CartService.DAL;
using eShop.CartService.DAL.Models;
using Microsoft.Extensions.Options;
using Mongo2Go;

namespace eShop.CartService.Test.IntegrationTests;

public class DatabaseFixture : IDisposable
{
    public DbContext DatabaseContext { get; private set; }

    private MongoDbRunner _runner;

    public DatabaseFixture()
    {
        _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false);

        var settings = new CartsDatabaseSettings()
        {
            ConnectionString = _runner.ConnectionString,
            DatabaseName = "Carts"
        };

        var options = Options.Create(settings);

        DatabaseContext = new DbContext(options);
    }

    public void Dispose()
    {
        _runner.Dispose();
    }
}
