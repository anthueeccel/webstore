using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Product;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.GetProductsByWebStore
{
    public class GetProductsByWebStoreHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetProductsByWebStoreHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<GetProductsByWebStoreResponse?> HandleAsync(GetProductsByWebStoreRequest request)
        {
            var webStoreExists = await _context.WebStores.AnyAsync(x => x.Id == request.WebStoreId);
            if (!webStoreExists)
            {
                _logger.LogWarning("WebStore {WebStoreId} not found.", request.WebStoreId);
                return null;
            }

            var products = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Where(x => x.WebStoreId == request.WebStoreId)
                .ToListAsync();

            return new GetProductsByWebStoreResponse
            {
                Products = products.Select(ProductDto.FromEntity).ToList()
            };
        }
    }
}
