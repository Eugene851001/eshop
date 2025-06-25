using AutoMapper;
using eShop.CartService.BLL.Interfaces;
using eShop.CartService.DAL.Mapping;
using eShop.CartService.DAL.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using CartItem = eShop.CartService.BLL.Models.CartItem;

namespace eShop.CartService.Test.IntegrationTests.DAL.Repositories;

internal class CartRepositoryTests
{
    private ICartsRepository _cartsRepository;
    private readonly DatabaseFixture _databaseFixture;

    private readonly IMapper _mapper;

    private struct TestData
    {
        public CartItem CartItem;
        public string CartId;
    }

    [TearDown]
    public void AfterTest()
    {
        var collection = _databaseFixture.DatabaseContext.Database.GetCollection<CartItem>("Carts");
        collection.DeleteMany(new BsonDocument());
    }

    [OneTimeTearDown]
    public void AfterTests()
    {
        _databaseFixture.Dispose();
    }

    public CartRepositoryTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CartsMappingProfile());
        });

        _mapper = mappingConfig.CreateMapper();

        _databaseFixture = new DatabaseFixture();
        _cartsRepository = new CartsRepository(_databaseFixture.DatabaseContext, _mapper);
    }

    [Test]
    public async Task GetCartItems_ShouldReturnCartItems()
    {
        // Arrange
        var expectedCarts = new TestData[]
        {
            new TestData { CartItem = new CartItem {Id = 1, Name = "Phone" }, CartId = "1" },
            new TestData { CartItem = new CartItem {Id = 2, Name = "DashCam" }, CartId = "1" },
            new TestData { CartItem = new CartItem {Id = 3, Name = "Tire" }, CartId = "2" },
        };

        foreach (var item in expectedCarts)
        {
            await _cartsRepository.AddCartItemAsync(item.CartItem, item.CartId);
        }

        // Act
        var result = await _cartsRepository.GetCartItemsAsync("2");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteCartItem_ItemsExists_ShouldDeleteCartItem()
    {
        //Arrange
        const string cartId = "2";

        var cartItem = new CartItem() { Id = 1, Name = "Name" };

        await _cartsRepository.AddCartItemAsync(cartItem, cartId);

        //Act
        var result = await _cartsRepository.RemoveCartItemAsync(cartItem.Id, cartId);

        //Assert
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public async Task DeleteCartItem_ItemsNotExists_ShouldNotDeleteCartItem()
    {
        //Arrange
        const string cartId = "99";

        var cartItem = new CartItem() { Id = 1, Name = "Name" };

        //Act
        var result = await _cartsRepository.RemoveCartItemAsync(cartItem.Id, cartId);

        //Assert
        Assert.That(result, Is.EqualTo(false));
    }
}
