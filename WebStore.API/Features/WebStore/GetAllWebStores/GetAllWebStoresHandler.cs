using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.WebStore;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.GetAllWebStores
{
    public class GetAllWebStoresHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetAllWebStoresHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(typeof(GetAllWebStoresHandler).FullName!);
        }

        public async Task<GetAllWebStoresResponse> HandleAsync(GetAllWebStoresRequest request)
        {
            var webStores = await _context.WebStores
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} web stores.", webStores.Count);

            return new GetAllWebStoresResponse
            {
                WebStores = webStores.Select(WebStoreDto.FromEntity).Where(x => x is not null).Select(x => x!).ToList()
            };
        }
    }
}
