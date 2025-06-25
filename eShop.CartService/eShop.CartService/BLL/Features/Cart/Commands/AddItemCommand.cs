using AutoMapper;
using eShop.CartService.BLL.DTOs;
using MediatR;

namespace eShop.CartService.BLL.Features.Cart.Commands;

public class AddItemCommand : IRequest<string>
{
    public string CartId { get; set; }

    public CartItemInfo Item { get; set; }
}

public class AddItemCommandHandler(Services.CartService cartService, IMapper mapper) : IRequestHandler<AddItemCommand, string>
{
    public async Task<string> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Models.CartItem>(request.Item);

        await cartService.AddCartItemAsync(entity, request.CartId);

        return request.CartId;
    }
}

