using eShop.CatalogService.BLL.Interfaces;
using eShop.CatalogService.DAL.Contexts;
using eShop.CatalogService.DAL.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.CatalogService.DAL;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(
                  //configuration["ConnectionString"],
                  configuration.GetConnectionString("DefaultConnection"),
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
    }
}
