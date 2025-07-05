using Reihan.Client.Authentication;
using Reihan.Client.Services;

namespace Reihan.Client.DependencyInjection
{
    public static class ClientDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductClient, ProductClient>();
            services.AddScoped<ICategoryClient, CategoryClient>();
            services.AddScoped<ICartClient, CartClient>();
            services.AddScoped<IAuthClient, AuthClient>();
            services.AddScoped<IUserClient, UserClient>();
            services.AddScoped<IOrderClient, OrderClient>();
            services.AddScoped<IDashboardClient, DashboardClient>();
            services.AddScoped<IFavoriteClient, FavoriteClient>();

            return services;
        }

        public static IServiceCollection AddHttp(this IServiceCollection services, string baseAddress)
        {
            services.AddScoped<AuthHeaderHandler>();

            services.AddHttpClient("AuthorizedClient", client =>
            { 
                client.BaseAddress = new Uri(baseAddress); 
            }).AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));

            return services;
        }
    }
}
