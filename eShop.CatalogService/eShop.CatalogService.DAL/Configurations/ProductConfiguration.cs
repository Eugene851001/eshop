using eShop.CatalogService.BLL.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.CatalogService.DAL.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(product => product.Name).IsRequired().HasMaxLength(50);
        builder.Property(product => product.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(product => product.Amount).IsRequired();
    }
}
