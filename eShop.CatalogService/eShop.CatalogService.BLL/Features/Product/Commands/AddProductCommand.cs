using AutoMapper;

using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Services;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Product.Commands;

public class AddProductCommand : IRequest<int>
{
    public required ProductInfo ProductInfo { get; set; }
}

public class AddProductCommandHander(
    ProductService productService,
    IMapper mapper) : IRequestHandler<AddProductCommand, int>
{
    public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Models.Product>(request.ProductInfo);

        await productService.AddAsync(entity);

        return entity.Id;
    }
}
