using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.DeleteWebStore
{
    public class DeleteWebStoreHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public DeleteWebStoreHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(typeof(DeleteWebStoreHandler).FullName!);
        }

        public async Task<DeleteWebStoreResponse?> HandleAsync(DeleteWebStoreRequest request)
        {
            var webStore = await _context.WebStores.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (webStore == null)
            {
                _logger.LogWarning("WebStore with id {WebStoreId} not found for deletion.", request.Id);
                return null;
            }

            _context.WebStores.Remove(webStore);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted WebStore {WebStoreId}.", request.Id);

            return new DeleteWebStoreResponse { Deleted = true };
        }
    }
}
