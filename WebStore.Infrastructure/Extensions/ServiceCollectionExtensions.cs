using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain.Repositories;
using WebStore.Infrastructure.Persistence;
using WebStore.Infrastructure.Repositories;
using WebStore.Infrastructure.Seeders;

namespace WebStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("WebStoreLocalDb");
            services.AddDbContext<WebStoreDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IWebStoreSeeder, WebStoreSeeder>();
            services.AddScoped<IWebStoreRepository, WebStoreRepository>();
        }
    }
}
