using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace eShop.CatalogService.BLL;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<Services.CategoryService>();
        services.AddScoped<Services.ProductService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
