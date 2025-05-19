
using Microsoft.Extensions.Logging;
using WebStore.Application.Dtos.Product;
using WebStore.Application.Dtos.WebStore;
using WebStore.Application.WebStore.Services;
using WebStore.Domain.Repositories;

namespace WebStore.Application.Services.WebStore
{
    internal class WebStoreService(IWebStoreRepository webStoreRepository, ILogger<WebStoreService> logger) : IWebStoreService
    {
        public async Task<IEnumerable<WebStoreDto>> GetAllWebStoresAsync()
        {
            logger.LogInformation("Fetching all web stores from the repository.");
            var webstores = await webStoreRepository.GetAllWebStoresAsync();

            logger.LogInformation("Mapping web stores to DTOs.");
            var webStoreDtos = webstores.Select(webStore => new WebStoreDto
            {
                Name = webStore.Name,
                Description = webStore.Description,
                HasDelivery = webStore.HasDelivery,
                Address = webStore.Address,
                ContactPhoneNumber = webStore.ContactPhoneNumber,
                ContactEmail = webStore.ContactEmail,
                ExtraInfo = webStore.ExtraInfo,
                Products = webStore.Products?.Select(product => new ProductDto
                {
                    Id = product.Id,
                    Category = product.Category.Name,
                    Brand = product.Brand.Name,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl
                }).ToList()
            });

            return webStoreDtos;
        }

        public async Task<WebStoreDto> GetWebStoreByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching web store with ID {Id} from the repository.", id);
            var webStore = await webStoreRepository.GetWebStoreByIdAsync(id);
            if (webStore == null)
            {
                logger.LogWarning("Web store with ID {Id} not found.", id);
                return null;
                //throw new KeyNotFoundException($"Web store with ID {id} not found.");
            }

            logger.LogInformation("Mapping web store to DTO.");
            return new WebStoreDto
            {
                Name = webStore.Name,
                Description = webStore.Description,
                HasDelivery = webStore.HasDelivery,
                Address = webStore.Address,
                ContactPhoneNumber = webStore.ContactPhoneNumber,
                ContactEmail = webStore.ContactEmail,
                ExtraInfo = webStore.ExtraInfo,
                Products = webStore.Products?.Select(product => new ProductDto
                {
                    Id = product.Id,
                    Category = product.Category.Name,
                    Brand = product.Brand.Name,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl
                }).ToList()
            };
        }
    }
}
