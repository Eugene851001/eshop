using eShop.CatalogService.API.Constants;
using eShop.CatalogService.API.Models;
using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Features.Category.Commands;
using eShop.CatalogService.BLL.Features.Category.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.CatalogService.API.Controllers.v1;

[Authorize]
public class CategoriesController : BaseApiController
{
    public CategoriesController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [Authorize(Policy = AuthPoliciesConstants.Read)]
    [ProducesResponseType(typeof(IEnumerable<CategoryInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IEnumerable<CategoryInfo>> GetCategories() => await Mediator.Send(new GetCategoriesQuery());

    [HttpPost]
    [Authorize(Policy = AuthPoliciesConstants.Edit)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<AddCategoryResponse> AddCategory([FromBody] AddCategoryCommand command)
    {
        var id = await Mediator.Send(command);

        var scheme = Request?.Scheme ?? "http";
        var response = new AddCategoryResponse
        {
            CategoryId = id,
            Links = [
                new HateoasLink { Method = "PUT", Href = Url.Action(nameof(UpdateCategory), null, new { id }, scheme) },
                new HateoasLink { Method = "DELETE", Href = Url.Action(nameof(DeleteCategory), null, new { id }, scheme) }
            ]
        };

        return response;
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AuthPoliciesConstants.Edit)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<int> UpdateCategory(
        [FromRoute] int id,
        [FromBody] UpdateCategoryCommand command)
    {
        command.CategoryInfo.Id = id;

        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPoliciesConstants.Edit)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var command = new RemoveCategoryCommand { Id = id };
        var isDeleted = await Mediator.Send(command);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
