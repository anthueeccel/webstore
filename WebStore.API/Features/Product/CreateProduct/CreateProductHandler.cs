using Microsoft.EntityFrameworkCore;
using WebStore.API.Shared.Dtos.Product;
using WebStore.Infrastructure.Persistence;
using ProductEntity = WebStore.Domain.Entities.Product;

namespace WebStore.API.Features.Product.CreateProduct
{
    public class CreateProductHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public CreateProductHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<CreateProductResponse?> HandleAsync(CreateProductRequest request)
        {
            if (!Guid.TryParse(request.CategoryId, out var categoryId) ||
                !Guid.TryParse(request.BrandId, out var brandId))
            {
                _logger.LogWarning("CreateProduct request contained invalid category or brand id.");
                return null;
            }

            var category = await _context.Categories.FindAsync(categoryId);
            var brand = await _context.Brands.FindAsync(brandId);
            var webStoreExists = await _context.WebStores.AnyAsync(x => x.Id == request.WebStoreId);
            if (category is null || brand is null || !webStoreExists)
            {
                _logger.LogWarning("CreateProduct dependency not found: Category={CategoryId}, Brand={BrandId}, WebStore={WebStoreId}.", request.CategoryId, request.BrandId, request.WebStoreId);
                return null;
            }

            var product = new ProductEntity
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = category,
                Brand = brand,
                Model = request.Model,
                ImageUrl = request.ImageUrl,
                WebStoreId = request.WebStoreId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created product {ProductId}.", product.Id);

            return new CreateProductResponse
            {
                Product = ProductDto.FromEntity(product)
            };
        }
    }
}
