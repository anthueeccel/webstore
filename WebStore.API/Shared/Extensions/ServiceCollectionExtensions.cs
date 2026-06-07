using FluentValidation;
using WebStore.API.Shared.Validators;

namespace WebStore.API.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            // Register all validators from the API assembly
            services.AddValidatorsFromAssemblyContaining<AddressValidator>();

            return services;
        }
    }
}
