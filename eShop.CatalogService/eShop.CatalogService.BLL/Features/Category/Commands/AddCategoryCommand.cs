using AutoMapper;

using eShop.CatalogService.BLL.Services;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Category.Commands;

public class AddCategoryCommand : IRequest<int>
{
    public required string Name { get; set; }
}

public class AddCategoryCommandHandler(CategoryService categoryService, IMapper mapper) : IRequestHandler<AddCategoryCommand, int>
{
    public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Models.Category>(request);
        await categoryService.AddAsync(entity);

        return entity.Id;
    }
}
