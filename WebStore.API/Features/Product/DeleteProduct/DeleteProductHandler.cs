using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.DeleteProduct
{
    public class DeleteProductHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public DeleteProductHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<DeleteProductResponse?> HandleAsync(DeleteProductRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product is null)
            {
                _logger.LogWarning("Product {ProductId} not found.", request.Id);
                return null;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted product {ProductId}.", request.Id);
            return new DeleteProductResponse { Deleted = true };
        }
    }
}
