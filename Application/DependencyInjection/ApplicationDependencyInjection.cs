using Application.Interfaces;
using Application.Interfaces.Application.Interfaces.Services;
using Application.Mappings;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IUserAddressService, UserAddressService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddAutoMapper(typeof(ApplicationMappingProfile).Assembly);

            return services;
        }
    }
}
