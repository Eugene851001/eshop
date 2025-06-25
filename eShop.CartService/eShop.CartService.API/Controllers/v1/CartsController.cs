using Asp.Versioning;
using eShop.CartService.BLL.DTOs;
using eShop.CartService.BLL.Features.Cart.Commands;
using eShop.CartService.BLL.Features.Cart.Queries;
using MediatR;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.CartService.API.Controllers.v1;

//[Authorize]
[ApiVersion("1")]
public class CartsController : BaseApiController
{
    public CartsController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Get cart info by id
    /// </summary>
    /// <param name="id">Id of the cart</param>
    /// <returns>Cart id and cart items</returns>
    [HttpGet("{id}")]
    public async Task<CartInfo> GetCartInfo(string id) => await Mediator.Send(new GetCartQuery { CartId = id });

    /// <summary>
    /// Adds item to the Carts
    /// </summary>
    /// <remarks>
    /// Sample request
    ///  POST /api/v1/carts/a1/items
    ///    {
    ///        "Name": "Racer Expert 29",
    ///        "Price": 500,
    ///        "Quantity": 1
    ///    }
    /// </remarks>
    /// <param name="cartId">Id of the cart</param>
    /// <param name="cartItem">Cart item to add to the cart</param>
    /// <returns>Id of the cart</returns>
    [HttpPost("{cartId}/items")]
    public async Task<string> AddCartItem(string cartId, [FromBody] CartItemInfo cartItem)
    {
        var command = new AddItemCommand { CartId = cartId, Item = cartItem };

        return await Mediator.Send(command);
    }

    /// <summary>
    /// Removes cart item from the cart
    /// </summary>
    /// <param name="cartId">Id of the cart</param>
    /// <param name="itemId">Id of the item in the cart</param>
    [HttpDelete("{cartId}/items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCartItem(string cartId, int itemId)
    {
        var command = new DeleteItemCommand { CartId = cartId, ItemId = itemId };
        var isDeleted = await Mediator.Send(command);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
