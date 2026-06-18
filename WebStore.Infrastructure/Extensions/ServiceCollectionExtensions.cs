using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Infrastructure.Persistence;
using WebStore.Infrastructure.Seeders;

namespace WebStore.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<WebStoreDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("WebStore.Infrastructure"))
            );

            services.AddScoped<IWebStoreSeeder, WebStoreSeeder>();
        }
    }
}
