using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Product;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.GetProduct
{
    public class GetProductHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetProductHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<GetProductResponse?> HandleAsync(GetProductRequest request)
        {
            var product = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            if (product is null)
            {
                _logger.LogWarning("Product {ProductId} not found.", request.Id);
                return null;
            }

            return new GetProductResponse
            {
                Product = ProductDto.FromEntity(product)
            };
        }
    }
}
