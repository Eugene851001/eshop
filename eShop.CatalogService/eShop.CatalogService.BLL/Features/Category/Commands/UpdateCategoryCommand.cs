using AutoMapper;

using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Services;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Category.Commands;

public class UpdateCategoryCommand : IRequest<int>
{
    public CategoryInfo CategoryInfo { get; set; }
}

public class UpdateCategoryCommandHandler(CategoryService categoryService, IMapper mapper) : IRequestHandler<UpdateCategoryCommand, int>
{
    public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<CategoryInfo, Models.Category>(request.CategoryInfo);
        await categoryService.UpdateAsync(entity);

        return entity.Id;
    }
}
