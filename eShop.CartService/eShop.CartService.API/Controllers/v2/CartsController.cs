using Asp.Versioning;
using eShop.CartService.BLL.DTOs;
using eShop.CartService.BLL.Features.Cart.Queries;
using MediatR;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.CartService.API.Controllers.v2;

//[Authorize]
[ApiVersion("2")]
public class CartsController : BaseApiController
{
    public CartsController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Returns cart items
    /// </summary>
    /// <param name="id">Id of the cart</param>
    /// <returns>Cart items</returns>
    [HttpGet("{id}")]
    public async Task<CartInfo> GetCartInfo(string id)
    {
        var result = await Mediator.Send(new GetCartQuery { CartId = id });

        return result;
    }
}
