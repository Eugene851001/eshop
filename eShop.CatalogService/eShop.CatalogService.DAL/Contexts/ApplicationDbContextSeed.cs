using eShop.CatalogService.BLL.Services;
using Microsoft.EntityFrameworkCore;

namespace eShop.CatalogService.DAL.Contexts;
public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context, CategoryService categoryService)
    {
        if (context.Database.IsSqlServer())
        {
            context.Database.Migrate();
        }

        var categories = await categoryService.GetAllAsync();

        if (!categories.Any()) 
        {
            await categoryService.AddAsync(new BLL.Models.Category { Name = "Sport" });
            await categoryService.AddAsync(new BLL.Models.Category { Name = "Tech" });
        } 
    }
}
