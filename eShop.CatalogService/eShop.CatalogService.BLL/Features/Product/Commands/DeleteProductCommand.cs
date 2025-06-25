using eShop.CatalogService.BLL.Services;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Product.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteProductCommandHandler(ProductService productService) : IRequestHandler<DeleteProductCommand, bool>
{
    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await productService.DeleteAsync(request.Id);
    }
}
