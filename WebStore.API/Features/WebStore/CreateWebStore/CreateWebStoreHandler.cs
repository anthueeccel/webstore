using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.API.Shared.Dtos.WebStore;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Persistence;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.API.Features.WebStore.CreateWebStore
{
    public class CreateWebStoreHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public CreateWebStoreHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(typeof(CreateWebStoreHandler).FullName!);
        }

        public async Task<CreateWebStoreResponse?> HandleAsync(CreateWebStoreRequest request)
        {
            var webStore = new WebStoreModel
            {
                Name = request.Name,
                Description = request.Description,
                HasDelivery = request.HasDelivery,
                Address = request.Address?.ToEntity(),
                ContactPhoneNumber = request.ContactPhoneNumber?.Trim(),
                ContactEmail = request.ContactEmail.Trim(),
                ExtraInfo = request.ExtraInfo,
                WebsiteUrl = request.WebsiteUrl?.Trim()
            };

            _context.WebStores.Add(webStore);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created WebStore {WebStoreId}", webStore.Id);

            return new CreateWebStoreResponse
            {
                WebStore = WebStoreDto.FromEntity(webStore)!
            };
        }
    }
}
