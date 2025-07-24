using Reihan.Shared.DTOs;
using Reihan.Shared.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Mappings
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            // Address
            CreateMap<Address, AddressDto>().ReverseMap();


            // User
            CreateMap<User, JwtUserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ReverseMap()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)));

            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, UpdateProfileRequest>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ReverseMap()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)));


            // Cart
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<CartItem, AddToCartRequest>().ReverseMap();


            // Order
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.ShippingAddress.City))
                .ForMember(d => d.ZipCode, o => o.MapFrom(s => s.ShippingAddress.ZipCode))
                .ReverseMap();

            CreateMap<Order, OrderDetailsDto>()
                .ForMember(d => d.State, o => o.MapFrom(s => s.ShippingAddress.State))
                .ForMember(d => d.City, o => o.MapFrom(s => s.ShippingAddress.City))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.ShippingAddress.Street))
                .ForMember(d => d.ZipCode, o => o.MapFrom(s => s.ShippingAddress.ZipCode))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductImage, o => o.MapFrom(s => s.ProductImage)); // مستقیم

            CreateMap<OrderItem, CartItemDto>().ReverseMap();
            CreateMap<CartItem, OrderItem>();

            // Product
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrls, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore())
                .ForMember(d => d.PriceAfterDiscount, o => o.MapFrom(s => s.GetPriceAfterDiscount()))
                .ReverseMap()
                .ForMember(dest => dest.Images, opt => opt.Ignore());


            // UserAddress
            CreateMap<UserAddress, UserAddressDto>().ReverseMap();
        }
    }
}