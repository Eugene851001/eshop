using AutoMapper;
using eShop.CartService.BLL.DTOs;
using MediatR;

namespace eShop.CartService.BLL.Features.Cart.Queries;

public class GetCartQuery : IRequest<CartInfo>
{
    public string CartId { get; set; }
}

public class GetCartQueryHandler(Services.CartService cartService, IMapper mapper) : IRequestHandler<GetCartQuery, CartInfo>
{
    public async Task<CartInfo> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var items = await cartService.GetCartItemsAsync(request.CartId);

        var result = new CartInfo
        {
            Id = request.CartId,
            Items = items.Select(item => mapper.Map<CartItemInfo>(item)).ToList()
        };

        return result;
    }
}
