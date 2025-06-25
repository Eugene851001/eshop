using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace eShop.CatalogService.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : Controller
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator;

    protected BaseApiController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
