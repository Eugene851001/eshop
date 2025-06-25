using eShop.CatalogService.API.Constants;
using eShop.CatalogService.API.Models;
using eShop.CatalogService.BLL.Features.Product.Commands;
using eShop.CatalogService.BLL.Features.Product.Queries;
using eShop.CatalogService.BLL.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ProductsResponseType = eShop.CatalogService.BLL.Wrappers.PagedResponse<System.Collections.Generic.IEnumerable<eShop.CatalogService.BLL.DTOs.ProductInfo>>;

namespace eShop.CatalogService.API.Controllers.v1;

[Authorize]
public class ProductsController : BaseApiController
{
    public ProductsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductsResponseType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ProductsResponseType> GetProducts([FromQuery] ProductsFilterModel filterModel) =>
        await Mediator.Send(new GetProductsQuery() { ProductsFilterModel = filterModel });

    [HttpPost]
    [Authorize(Policy = AuthPoliciesConstants.Edit)]
    [ProducesResponseType(typeof(AddCategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<AddCategoryResponse> AddProduct([FromBody] AddProductCommand addCommand)
    {
        var id = await Mediator.Send(addCommand);

        var response = new AddCategoryResponse
        {
            CategoryId = id,
            Links = [
                new HateoasLink { Method = "PUT", Href = Url.Action(nameof(UpdateProduct), null, new { id }, Request.Scheme) },
                new HateoasLink { Method = "DELETE", Href = Url.Action(nameof(DeleteProduct), null, new { id }, Request.Scheme) }
            ]
        };

        return response;
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AuthPoliciesConstants.Edit)]
    [ProducesResponseType(typeof(BLL.DTOs.ProductInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<BLL.DTOs.ProductInfo> UpdateProduct(
        [FromRoute] int id,
        [FromBody] UpdateProductCommand updateCommand)
    {
        updateCommand.ProductId = id;
        return await Mediator.Send(updateCommand);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPoliciesConstants.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromQuery] int id)
    {
        var command = new DeleteProductCommand { Id = id };
        var isDeleted = await Mediator.Send(command);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
