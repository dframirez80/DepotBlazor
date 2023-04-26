using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Domain
{
    public static class DomainIoC
    {
        public static IServiceCollection AddAppDomain(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IDepotService, DepotService>();
            services.AddScoped<IProductMovementService, ProductMovementService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}