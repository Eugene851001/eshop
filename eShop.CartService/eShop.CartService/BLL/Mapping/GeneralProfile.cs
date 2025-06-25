using AutoMapper;

namespace eShop.CartService.BLL.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<Models.Cart, DTOs.CartInfo>();

        CreateMap<Models.CartItem, DTOs.CartItemInfo>();
        CreateMap<DTOs.CartItemInfo, Models.CartItem>();
    }
}
