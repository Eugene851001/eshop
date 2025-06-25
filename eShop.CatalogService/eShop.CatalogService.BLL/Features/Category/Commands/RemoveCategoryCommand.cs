using System.Linq.Expressions;

using eShop.CatalogService.BLL.Services;

using LinqKit;

using MediatR;

namespace eShop.CatalogService.BLL.Features.Category.Commands;

public class RemoveCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class RemoveCategoryCommandHandler(CategoryService categoryService, ProductService productService) : IRequestHandler<RemoveCategoryCommand, bool>
{
    public async Task<bool> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var predicate = GetFilterExpression(request.Id);

        await productService.DeleteManyAsync(predicate);

        return await categoryService.DeleteAsync(request.Id);
    }

    private static Expression<Func<Models.Product, bool>> GetFilterExpression(int categoryId)
    {
        var predicate = PredicateBuilder.New<Models.Product>();
        predicate.Or(x => x.CategoryId == categoryId);

        return predicate;
    }
}
