using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : Controller
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator;

    public BaseApiController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
