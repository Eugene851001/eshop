using eShop.CartService.API.Controllers.v1;
using eShop.CartService.BLL.DTOs;
using eShop.CartService.BLL.Features.Cart.Commands;
using eShop.CartService.BLL.Features.Cart.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace eShop.CartService.API.Tests;

public class CartsControllerTests
{
    [Test]
    public async Task CartsController_GetCartItsm_ShouldCallMediator()
    {
        // Arrange
        var cartId = Guid.NewGuid().ToString();

        var mockMediator = new Mock<IMediator>();
        mockMediator.Setup(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()));

        var controller = new CartsController(mockMediator.Object);

        // Act
        var result = await controller.GetCartInfo(cartId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;

        mockMediator.Verify(m => m.Send(It.Is<GetCartQuery>(q => q.CartId == cartId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CartsController_AddCartItem_ShouldCallMediator()
    {
        // Arrange
        var cartId = Guid.NewGuid().ToString();
        var cartItem = new CartItemInfo { Name = "Test Item", Price = 10.0m, Quantity = 1 };

        var mockMediator = new Mock<IMediator>();
        mockMediator.Setup(m => m.Send(It.IsAny<AddItemCommand>(), It.IsAny<CancellationToken>()));

        var controller = new CartsController(mockMediator.Object);

        // Act
        var result = await controller.AddCartItem(cartId, cartItem);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;

        mockMediator.Verify(m => m.Send(It.Is<AddItemCommand>(c => c.CartId == cartId && c.Item == cartItem), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CartsController_DeleteCartItem_ShouldCallMediator()
    {
        // Arrange
        var cartId = Guid.NewGuid().ToString();
        var itemId = 1;

        var mockMediator = new Mock<IMediator>();
        mockMediator.Setup(m => m.Send(It.IsAny<DeleteItemCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));

        var controller = new CartsController(mockMediator.Object);

        // Act
        var result = await controller.DeleteCartItem(cartId, itemId);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
        var okResult = result as OkResult;

        mockMediator.Verify(m => m.Send(It.Is<DeleteItemCommand>(c => c.CartId == cartId && c.ItemId == itemId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
