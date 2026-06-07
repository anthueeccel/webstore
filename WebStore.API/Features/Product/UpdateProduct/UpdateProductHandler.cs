using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Product;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.UpdateProduct
{
    public class UpdateProductHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public UpdateProductHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<UpdateProductResponse?> HandleAsync(UpdateProductRequest request)
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

            if (!Guid.TryParse(request.CategoryId, out var categoryId) ||
                !Guid.TryParse(request.BrandId, out var brandId))
            {
                _logger.LogWarning("UpdateProduct request contained invalid category or brand id.");
                return null;
            }

            var category = await _context.Categories.FindAsync(categoryId);
            var brand = await _context.Brands.FindAsync(brandId);
            var webStoreExists = await _context.WebStores.AnyAsync(x => x.Id == request.WebStoreId);
            if (category is null || brand is null || !webStoreExists)
            {
                _logger.LogWarning("UpdateProduct dependency not found: Category={CategoryId}, Brand={BrandId}, WebStore={WebStoreId}.", request.CategoryId, request.BrandId, request.WebStoreId);
                return null;
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Category = category;
            product.Brand = brand;
            product.Model = request.Model;
            product.ImageUrl = request.ImageUrl;
            product.WebStoreId = request.WebStoreId;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated product {ProductId}.", product.Id);

            return new UpdateProductResponse
            {
                Product = ProductDto.FromEntity(product)
            };
        }
    }
}
