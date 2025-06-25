using AutoMapper;

namespace eShop.CartService.DAL.Mapping;

public class CartsMappingProfile : Profile
{
    public CartsMappingProfile()
    {
        CreateMap<BLL.Models.CartItem, DAL.Models.CartItem>()
            .ForMember(dbItem => dbItem.OriginalId, opt => opt.MapFrom(x => x.Id))
            .ForMember(dbItem => dbItem.Id, opt => opt.Ignore());

        CreateMap<DAL.Models.CartItem, BLL.Models.CartItem>()
           .ForMember(item => item.Id, opt => opt.MapFrom(x => x.OriginalId));
    }
}
