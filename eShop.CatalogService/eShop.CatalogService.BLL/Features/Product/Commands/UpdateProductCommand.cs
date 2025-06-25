using AutoMapper;

using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.BLL.Models;
using eShop.CatalogService.BLL.Services;

using MediatR;

using Microsoft.Extensions.Logging;

namespace eShop.CatalogService.BLL.Features.Product.Commands;

public class UpdateProductCommand : IRequest<ProductInfo>
{
    public int ProductId { get; set; }

    public required ProductInfo ProductInfo { get; set; }
}

public class UpdateProductCommandHanlder(
    ProductService productService,
    ILogger<UpdateProductCommand> logger,
    IMapper mapper,
    IMessageProducer<ProductUpdatedMessage> producer) : IRequestHandler<UpdateProductCommand, ProductInfo>
{
    public async Task<ProductInfo> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Models.Product>(request.ProductInfo);
        entity.Id = request.ProductId;

        await productService.UpdateAsync(entity);

        try
        {
            var message = mapper.Map<ProductUpdatedMessage>(entity);
            await producer.Produce(message);
        }
        catch (Exception ex)
        {
            logger.LogError("Can not produce message: {0}", ex.Message);
        }

        var result = mapper.Map<ProductInfo>(entity);

        return result;
    }
}
