using AutoMapper;
using eShop.CartService.BLL.DTOs;
using MediatR;


namespace eShop.CartService.BLL.Features.Cart.Commands;

public class UpdateItemCommand : IRequest<int>
{
    public required CartItemInfo CartItem { get; set; }
}

public class UpdatItemCommandHandler(BLL.Services.CartService cartService, IMapper mapper) : IRequestHandler<UpdateItemCommand, int>
{
    public async Task<int> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Models.CartItem>(request.CartItem);

        await cartService.UpdateCartItemAsync(entity);

        return request.CartItem.Id;
    }
}
