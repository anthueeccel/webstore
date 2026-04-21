using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Repositories;
using ProductModel = WebStore.Domain.Entities.Product;

namespace WebStore.Application.Commands.Product
{
    public class CreateProductCommandHandler(
                ILogger<CreateProductCommandHandler> logger,
                ICategoryRepository categoryRepository,
                IBrandRepository brandRepository,
                IProductRepository productRepository) : IRequestHandler<CreateProductCommand, Guid>
    {
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new product with name {Name}.", request.Name);

            var brand = brandRepository.GetBrandByIdAsync(Guid.Parse(request.BrandId))
                .GetAwaiter()
                .GetResult();
            var category = categoryRepository.GetCategoryByIdAsync(Guid.Parse(request.CategoryId))
                .GetAwaiter()
                .GetResult();

            if (brand is null)
            {
                logger.LogWarning("Brand not found.");
                return Guid.Empty;
            }
            if (category is null)
            {
                logger.LogWarning("Category not found.");
                return Guid.Empty;
            }
            if (string.IsNullOrWhiteSpace(request.Name) || request.Price <= 0)
            {
                logger.LogWarning("Invalid product details.");
                return Guid.Empty;
            }
            var product = new ProductModel
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

            logger.LogInformation("Saving product to the repository.");
            var createdProduct = await productRepository.CreateProductAsync(product!);

            if (createdProduct == null)
            {
                logger.LogWarning("Failed to create product.");
                return Guid.Empty;
            }

            logger.LogInformation("Product created successfully with ID {Id}.", product!.Id);
            return createdProduct.Id;
        }
    }
}