using System.Reflection;
using eShop.CartService.BLL.Interfaces;
using eShop.CartService.DAL;
using eShop.CartService.DAL.Models;
using eShop.CartService.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.CartService;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<BLL.Services.CartService>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(conf =>
        {
            conf.AddProfile<BLL.Mapping.GeneralProfile>();
        });

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new CartsDatabaseSettings();

        configuration.GetSection("MongoDB").Bind(options);
        services.Configure<CartsDatabaseSettings>(opt =>
        {
            opt.ConnectionString = options.ConnectionString;
            opt.DatabaseName = options.DatabaseName;
        });

        services.AddScoped<DbContext>();
        services.AddScoped<ICartsRepository, CartsRepository>();

        return services;
    }
}
