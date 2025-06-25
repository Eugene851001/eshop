using MediatR;

namespace eShop.CartService.BLL.Features.Cart.Commands;

public class DeleteItemCommand : IRequest<bool>
{
    public required string CartId { get; set; }

    public int ItemId { get; set; }
}

public class DeleteItemCommandHandler(Services.CartService cartService) : IRequestHandler<DeleteItemCommand, bool>
{
    public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
    {
        return await cartService.DeleteCartItemAsync(request.ItemId, request.CartId);
    }
}
