using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.API.Shared.Dtos.WebStore;
using WebStore.Infrastructure.Persistence;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.API.Features.WebStore.UpdateWebStore
{
    public class UpdateWebStoreHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public UpdateWebStoreHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(typeof(UpdateWebStoreHandler).FullName!);
        }

        public async Task<UpdateWebStoreResponse?> HandleAsync(UpdateWebStoreRequest request)
        {
            var webStore = await _context.WebStores.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (webStore == null)
            {
                _logger.LogWarning("WebStore with id {WebStoreId} was not found for update.", request.Id);
                return null;
            }

            webStore.Name = request.Name;
            webStore.Description = request.Description;
            webStore.HasDelivery = request.HasDelivery;
            webStore.Address = request.Address?.ToEntity();
            webStore.ContactPhoneNumber = request.ContactPhoneNumber?.Trim();
            webStore.ContactEmail = request.ContactEmail.Trim();
            webStore.ExtraInfo = request.ExtraInfo;
            webStore.WebsiteUrl = request.WebsiteUrl?.Trim();

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated WebStore {WebStoreId}.", webStore.Id);

            return new UpdateWebStoreResponse
            {
                WebStore = WebStoreDto.FromEntity(webStore)!
            };
        }
    }
}
