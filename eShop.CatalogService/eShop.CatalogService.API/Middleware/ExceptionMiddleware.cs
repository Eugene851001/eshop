using eShop.CatalogService.BLL.Exceptions;

namespace eShop.CatalogService.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (EntityNotFoundException exception)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(exception.Message);
        }
    }
}
