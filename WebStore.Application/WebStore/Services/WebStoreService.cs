
using Microsoft.Extensions.Logging;
using WebStore.Domain.Repositories;

namespace WebStore.Application.WebStore.Services
{
    internal class WebStoreService(IWebStoreRepository webStoreRepository, ILogger<WebStoreService> logger) : IWebStoreService
    {
        public async Task<IEnumerable<Domain.Entities.WebStore>> GetAllWebStoresAsync()
        {
            logger.LogInformation("Fetching all web stores from the repository.");
            return await webStoreRepository.GetAllWebStoresAsync();
        }

        public Task<Domain.Entities.WebStore> GetWebStoreByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching web store with ID {Id} from the repository.", id);
            return webStoreRepository.GetWebStoreByIdAsync(id);
        }
    }
}
