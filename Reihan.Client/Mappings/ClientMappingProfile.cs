using AutoMapper;
using Reihan.Client.Models;

namespace Reihan.Client.Mappings
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {
            CreateMap<OrderItemDto, CartItemDto>().ReverseMap();
        }
    }
}
