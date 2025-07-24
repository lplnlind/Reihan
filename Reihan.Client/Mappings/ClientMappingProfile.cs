using AutoMapper;
using Reihan.Client.Pages.Common;
using Reihan.Shared.DTOs;

namespace Reihan.Client.Mappings
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<OrderItemDto, CartItemDto>().ReverseMap();

            CreateMap<UserProfileDto, UpdateProfileRequest>().ReverseMap();

            CreateMap<UserAddressDto, AddressDto>();

            // Order Summary Component 
            CreateMap<CartDto, OrderSummaryModel>();
            CreateMap<OrderDetailsDto, OrderSummaryModel>();
        }
    }
}
