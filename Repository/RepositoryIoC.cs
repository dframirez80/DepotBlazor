using Domain.Constants;
using Domain.Repository;
using DomainRepository.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository.EntityDbContext;
using Repository.Repositories;

namespace Repository
{
    public static class RepositoryIoC
    {
        public static IServiceCollection AddAppRepository(this IServiceCollection services, IConfiguration config)
        {
            services
               .AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(config.GetConnectionString(SettingsConfig.ConnectionString)));
            services
                .AddScoped<IUoW, UoW>()
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            return services;
        }
    }
}