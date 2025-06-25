using eShop.CartService.BLL.Interfaces;
using eShop.CartService.BLL.Models;
using Moq;

namespace eShop.CartService.Test.BLL.Services;

internal class CartServiceTests
{
    private const string CartId = "1";

    private Mock<ICartsRepository> _mockCartsRepository;
    private CartService.BLL.Services.CartService _cartService;

    [OneTimeSetUp]
    public void BeforeTests()
    {
        _mockCartsRepository = new Mock<ICartsRepository>();
        _cartService = new CartService.BLL.Services.CartService(_mockCartsRepository.Object);
    }

    [TearDown]
    public void AfterEachTest()
    {
        _mockCartsRepository.Invocations.Clear();
    }

    [Test]
    public void GetCarItem_CartIdNull_ThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _cartService.GetCartItemsAsync(null));
    }

    [Test]
    public void GetCarItem_CartIdEmpty_ThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _cartService.GetCartItemsAsync(string.Empty));
    }

    [Test]
    public void AddCartItem_ImageUrlInvalid_ThrowArgumentException()
    {

        var cartItem = new CartItem { Id = 1, Name = "Phone", ImageUrl = "invalid_url" };

        Assert.Throws<ArgumentException>(() => _cartService.AddCartItemAsync(cartItem, CartId));
    }

    [Test]
    public async Task GetCartItems_ShouldReturnCartItems()
    {
        // Arrange          
        var expectedCarts = new List<CartItem>
        {
            new CartItem { Id = 1, Name = "Phone"},
            new CartItem { Id = 2, Name = "Laptop"}
        };

        _mockCartsRepository.Setup(repo => repo.GetCartItemsAsync(CartId)).Returns(Task.FromResult(expectedCarts.AsEnumerable()));

        // Act
        var result = await _cartService.GetCartItemsAsync(CartId);

        // Assert
        Assert.That(result, Is.EqualTo(expectedCarts));

        _mockCartsRepository.Verify(repo => repo.GetCartItemsAsync(CartId), Times.Once);
    }

    [Test]
    public void AddCartItem_ShouldAddCartItem()
    {
        // Arrange
        var cartItem = new CartItem { Id = 1, Name = "Phone" };

        _mockCartsRepository.Setup(repo => repo.AddCartItemAsync(cartItem, CartId));

        // Act
        _cartService.AddCartItemAsync(cartItem, CartId);

        // Assert
        _mockCartsRepository.Verify(repo => repo.AddCartItemAsync(cartItem, CartId), Times.Once);
    }

    [Test]
    public async Task DeleteCartItem_ShouldDeleteCartItem()
    {
        //Arrange
        var cartItem = new CartItem { Id = 1, Name = "Phone" };

        _mockCartsRepository.Setup(repo => repo.RemoveCartItemAsync(cartItem.Id, CartId)).Returns(Task.FromResult(true));

        //Act
        var isDeleted = await _cartService.DeleteCartItemAsync(cartItem.Id, CartId);

        //Assert
        Assert.True(isDeleted);
        _mockCartsRepository.Verify(repo => repo.RemoveCartItemAsync(cartItem.Id, CartId), Times.Once);
    }

    [Test]
    public async Task DeleteCartItem_ShouldNotDeleteCartItem()
    {
        //Arrange
        var cartItem = new CartItem { Id = 1, Name = "Phone" };

        _mockCartsRepository.Setup(repo => repo.RemoveCartItemAsync(cartItem.Id, CartId)).Returns(Task.FromResult(false));

        //Act
        var isDeleted = await _cartService.DeleteCartItemAsync(cartItem.Id, CartId);

        //Assert
        Assert.False(isDeleted);
        _mockCartsRepository.Verify(repo => repo.RemoveCartItemAsync(cartItem.Id, CartId), Times.Once);
    }
}
