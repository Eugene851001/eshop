using System.Linq.Expressions;

using AutoMapper;

using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Models;
using eShop.CatalogService.BLL.Services;
using eShop.CatalogService.BLL.Wrappers;

using LinqKit;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Product.Queries;

public class GetProductsQuery : IRequest<PagedResponse<IEnumerable<ProductInfo>>>
{
    public ProductsFilterModel? ProductsFilterModel { get; set; }
}

public class GetProductsQueryHandler(ProductService productService, IMapper mapper) : IRequestHandler<GetProductsQuery, PagedResponse<IEnumerable<ProductInfo>>>
{
    public async Task<PagedResponse<IEnumerable<ProductInfo>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var filterExpression = GetFilterExpression(request.ProductsFilterModel);
        int skip = (request.ProductsFilterModel.PageNumber - 1) * request.ProductsFilterModel.PageSize;

        var items = await productService.GetAllAsync(filterExpression, skip, request.ProductsFilterModel.PageSize);

        return new PagedResponse<IEnumerable<ProductInfo>>
        {
            PageNumber = request.ProductsFilterModel?.PageNumber ?? -1,
            PageSize = request.ProductsFilterModel?.PageSize ?? -1,
            Data = items.Select(x => mapper.Map<ProductInfo>(x)).ToList()
        };
    }

    private static Expression<Func<Models.Product, bool>>? GetFilterExpression(ProductsFilterModel? filter)
    {
        if (filter == null || filter.CategoryId == null) return null;

        var predicate = PredicateBuilder.New<Models.Product>();
        predicate.Or(x => x.CategoryId == filter.CategoryId);

        return predicate;
    }
}
