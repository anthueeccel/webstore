using Microsoft.Extensions.DependencyInjection;
using WebStore.Application.WebStore.Services;

namespace WebStore.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWebStoreService, WebStoreService>();
        }
    }
}
