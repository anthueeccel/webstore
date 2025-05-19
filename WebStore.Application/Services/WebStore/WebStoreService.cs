
using Microsoft.Extensions.Logging;
using WebStore.Application.Dtos.Product;
using WebStore.Application.Dtos.WebStore;
using WebStore.Application.WebStore.Services;
using WebStore.Domain.Repositories;

namespace WebStore.Application.Services.WebStore
{
    internal class WebStoreService(IWebStoreRepository webStoreRepository, ILogger<WebStoreService> logger) : IWebStoreService
    {
        public async Task<IEnumerable<WebStoreDto?>> GetAllWebStoresAsync()
        {
            logger.LogInformation("Fetching all web stores from the repository.");
            var webstores = await webStoreRepository.GetAllWebStoresAsync();

            if (webstores == null || !webstores.Any())
            {
                logger.LogWarning("No web stores found in the repository.");
                return null;
            }

            logger.LogInformation("Mapping web stores to DTOs.");
            var webStoreDtos = webstores.Select(WebStoreDto.FromEntity);

            return webStoreDtos;
        }

        public async Task<WebStoreDto?> GetWebStoreByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching web store with ID {Id} from the repository.", id);
            var webStore = await webStoreRepository.GetWebStoreByIdAsync(id);
            if (webStore == null)
            {
                logger.LogWarning("Web store with ID {Id} not found.", id);
                return null;
            }

            logger.LogInformation("Mapping web store to DTO.");
            return WebStoreDto.FromEntity(webStore);
        }
    }
}
