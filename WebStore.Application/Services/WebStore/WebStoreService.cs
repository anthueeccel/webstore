
using Microsoft.Extensions.Logging;
using WebStore.Application.Dtos.Commom;
using WebStore.Application.Dtos.WebStore;
using WebStore.Domain.Repositories;

namespace WebStore.Application.Services.WebStore
{
    internal class WebStoreService(IWebStoreRepository webStoreRepository, ILogger<WebStoreService> logger) : IWebStoreService
    {
        public Task<WebStoreDto?> CreateWebStoreAsync(WebStoreCreateDto webStoreCreateDto)
        {
            logger.LogInformation("Creating a new web store with name {Name}.", webStoreCreateDto.Name);
            var webStore = WebStoreCreateDto.ToEntity(webStoreCreateDto);

            logger.LogInformation("Saving web store to the repository.");
            var createdWebStore = webStoreRepository.CreateWebStoreAsync(webStore).Result;
            if (createdWebStore == null)
            {
                logger.LogWarning("Failed to create web store.");
                return Task.FromResult<WebStoreDto?>(null);
            }
            logger.LogInformation("Web store created successfully with ID {Id}.", webStore.Id);
            return Task.FromResult<WebStoreDto?>(WebStoreDto.FromEntity(webStore));
        }

        public async Task<IEnumerable<WebStoreDto?>?> GetAllWebStoresAsync()
        {
            logger.LogInformation("Fetching all web stores from the repository.");
            var webstores = await webStoreRepository.GetAllWebStoresAsync();

            if (webstores == null || !webstores.Any())
            {
                logger.LogWarning("No web stores found in the repository.");
                return (IEnumerable<WebStoreDto?>?)[];
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

        public async Task<WebStoreDto?> UpdateWebStoreAsync(WebStoreUpdateDto webStoreDto)
        {
            logger.LogInformation("Fetching web store with ID {Id} from the repository.", webStoreDto.Id);
            var webStore = await webStoreRepository.GetWebStoreByIdAsync(webStoreDto.Id);
            if (webStore == null)
            {
                logger.LogWarning("Web store with ID {Id} not found.", webStoreDto.Id);
                return null;
            }
            webStore.Name = webStoreDto.Name;
            webStore.Description = webStoreDto.Description;
            webStore.HasDelivery = webStoreDto.HasDelivery;
            webStore.Address = webStoreDto?.Address?.ToEntity();
            webStore.ContactPhoneNumber = webStoreDto?.ContactPhoneNumber;
            webStore.ContactEmail = webStoreDto!.ContactEmail;
            webStore.ExtraInfo = webStoreDto.ExtraInfo;
            webStore.WebsiteUrl = webStoreDto.WebsiteUrl;
            logger.LogInformation("Updating web store with ID {Id} in the repository.", webStore.Id);
            var updatedWebStore = await webStoreRepository.UpdateWebStoreAsync(webStore);
            if (updatedWebStore == null)
            {
                logger.LogWarning("Failed to update web store with ID {Id}.", webStore.Id);
                return null;
            }
            logger.LogInformation("Web store with ID {Id} updated successfully.", webStore.Id);
            return await Task.FromResult<WebStoreDto?>(WebStoreDto.FromEntity(webStore));
        }

        public Task DeleteWebStoreAsync(Guid id)
        {
            logger.LogInformation("Deleting web store with ID {Id}.", id);
            return webStoreRepository.DeleteWebStoreAsync(id);
        }
    }
}
