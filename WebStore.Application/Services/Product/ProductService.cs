using Microsoft.Extensions.Logging;
using WebStore.Application.Dtos.Product;
using WebStore.Domain.Repositories;

namespace WebStore.Application.Services.Product
{
    internal class ProductService(
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository,
        ILogger<ProductService> logger) : IProductService
    {
        public Task<ProductDto?> CreateProductAsync(Guid webStoreId, ProductCreateDto productCreateDto)
        {
            logger.LogInformation("Creating a new Product with name {Name}.", productCreateDto.Name);
            var product = ProductCreateDto.ToEntity(brandRepository, categoryRepository, productCreateDto);
            product!.WebStoreId = webStoreId;

            logger.LogInformation("Saving Product to the repository.");
            var createdProduct = productRepository.CreateProductAsync(product).Result;
            if (createdProduct == null)
            {
                logger.LogWarning("Failed to create Product.");
                return Task.FromResult<ProductDto?>(null);
            }
            logger.LogInformation("Product created successfully with ID {Id}.", product.Id);
            return Task.FromResult<ProductDto?>(ProductDto.FromEntity(product));
        }

        public async Task<IEnumerable<ProductDto?>> GetAllProductsFromWebStoreAsync(Guid webStoreId)
        {
            logger.LogInformation("Fetching all Products from the repository.");
            var products = await productRepository.GetAllProductsFromWebStoreAsync(webStoreId);

            if (products == null || !products.Any())
            {
                logger.LogWarning("No Products found in the repository.");
                return Enumerable.Empty<ProductDto?>();
            }

            logger.LogInformation("Mapping Products to DTOs.");
            var productDtos = products.Select(ProductDto.FromEntity);

            return productDtos;
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            logger.LogInformation("Fetching Product with ID {Id} from the repository.", id);
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                logger.LogWarning("Product with ID {Id} not found.", id);
                return null;
            }

            logger.LogInformation("Mapping Product to DTO.");
            return ProductDto.FromEntity(product);
        }

        public async Task<ProductDto?> UpdateProductAsync(ProductUpdateDto productDto)
        {
            logger.LogInformation("Fetching Product with ID {Id} from the repository.", productDto.Id);
            var product = await productRepository.GetProductByIdAsync(productDto.Id);
            if (product == null)
            {
                logger.LogWarning("Product with ID {Id} not found.", productDto.Id);
                return null;
            }
            if(product.Brand.Id != Guid.Parse(productDto.BrandId) || product.Category.Id != Guid.Parse(productDto.CategoryId))
            {
                logger.LogInformation("Fetching Brand and Category for Product with ID {Id}.", productDto.Id);
                var brand = await brandRepository.GetBrandByIdAsync(Guid.Parse(productDto.BrandId));
                if (brand == null)
                {
                    logger.LogWarning("Brand with ID {BrandId} not found.", productDto.BrandId);
                    return null;
                }
                var category = await categoryRepository.GetCategoryByIdAsync(Guid.Parse(productDto.CategoryId));
                if (category == null)
                {
                    logger.LogWarning("Category with ID {CategoryId} not found.", productDto.CategoryId);
                    return null;
                }
                product.Brand = brand;
                product.Category = category;
            }  
            product.Id = productDto.Id;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Brand = product.Brand;
            product.Category = product.Category;
            product.Price = productDto.Price;
            product.ImageUrl = productDto.ImageUrl;
            logger.LogInformation("Updating Product with ID {Id} in the repository.", product.Id);
            var updatedProduct = await productRepository.UpdateProductAsync(product);
            if (updatedProduct == null)
            {
                logger.LogWarning("Failed to update Product with ID {Id}.", product.Id);
                return null;
            }
            logger.LogInformation("Product with ID {Id} updated successfully.", product.Id);
            return await Task.FromResult<ProductDto?>(ProductDto.FromEntity(product));
        }

        public Task DeleteProductAsync(Guid id)
        {
            logger.LogInformation("Deleting Product with ID {Id}.", id);
            return productRepository.DeleteProductAsync(id);
        }
    }
}
