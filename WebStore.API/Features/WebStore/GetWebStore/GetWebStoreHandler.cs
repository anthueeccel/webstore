using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.WebStore;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.GetWebStore
{
    public class GetWebStoreHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetWebStoreHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(typeof(GetWebStoreHandler).FullName!);
        }

        public async Task<GetWebStoreResponse?> HandleAsync(GetWebStoreRequest request)
        {
            var webStore = await _context.WebStores
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (webStore == null)
            {
                _logger.LogWarning("WebStore with id {WebStoreId} not found.", request.Id);
                return null;
            }

            return new GetWebStoreResponse
            {
                WebStore = WebStoreDto.FromEntity(webStore)
            };
        }
    }
}
