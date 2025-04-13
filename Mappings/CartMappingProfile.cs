// In myShopAPI/Mappings/CartMappingProfile.cs
using AutoMapper;
using myShopAPI.Models;
using myShopAPI.Transport;

namespace myShopAPI.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            // Mapping von Cart zu CreateCartDto
            CreateMap<Cart, CreateCartDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id));

            // Weitere Mappings hier hinzufügen, falls nötig
            CreateMap<Cart, CartItemDto>();
        }
    }
}