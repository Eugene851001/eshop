using System.Reflection;

using eShop.CatalogService.BLL.Models;

using Microsoft.EntityFrameworkCore;

namespace eShop.CatalogService.DAL.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
