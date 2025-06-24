using Microsoft.Extensions.DependencyInjection;
using WebStore.Application.Services.Product;
using WebStore.Application.Services.WebStore;

namespace WebStore.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWebStoreService, WebStoreService>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
