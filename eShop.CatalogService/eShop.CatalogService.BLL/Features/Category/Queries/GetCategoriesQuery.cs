using AutoMapper;

using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Services;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Category.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryInfo>>
{
}

public class GetCategoriesQueryHadler(CategoryService categoryService, IMapper mapper) : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryInfo>>
{
    public async Task<IEnumerable<CategoryInfo>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllAsync();

        return categories.Select(x => mapper.Map<CategoryInfo>(x));
    }
}
