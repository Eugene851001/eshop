using AutoMapper;

using eShop.CatalogService.BLL.DTOs;
using eShop.CatalogService.BLL.Features.Category.Commands;
using eShop.CatalogService.BLL.Models;

namespace eShop.CatalogService.BLL.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<AddCategoryCommand, Category>();
        CreateMap<Category, CategoryInfo>();

        CreateMap<Product, ProductInfo>();

        CreateMap<ProductInfo, Product>();

        CreateMap<Product, ProductUpdatedMessage>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Amount));
    }
}
