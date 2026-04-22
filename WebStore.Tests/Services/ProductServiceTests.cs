using Microsoft.Extensions.Logging;
using WebStore.Application.Dtos.Product;
using WebStore.Application.Services.Product;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;

namespace WebStore.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IBrandRepository> _mockBrandRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockBrandRepository = new Mock<IBrandRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();

            _productService = new ProductService(
                _mockProductRepository.Object,
                _mockBrandRepository.Object,
                _mockCategoryRepository.Object,
                _mockLogger.Object);
        }

        #region CreateProductAsync Tests

        [Fact]
        public async Task CreateProductAsync_WithValidInput_ReturnsCreatedProductDto()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var createDto = new ProductCreateDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = categoryId.ToString(),
                BrandId = brandId.ToString(),
                Model = "Model X",
                ImageUrl = "https://test.com/image.jpg",
                WebStoreId = webStoreId
            };

            var brand = new Brand { Id = brandId, Name = "Test Brand" };
            var category = new Category { Id = categoryId, Name = "Test Category" };

            var createdProduct = new Product
            {
                Id = productId,
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Brand = brand,
                Category = category,
                Model = createDto.Model,
                ImageUrl = createDto.ImageUrl,
                WebStoreId = webStoreId
            };

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync(new List<Product>());

            _mockBrandRepository.Setup(r => r.GetBrandByIdAsync(brandId))
                .ReturnsAsync(brand);

            _mockCategoryRepository.Setup(r => r.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockProductRepository.Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productService.CreateProductAsync(webStoreId, createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdProduct.Name, result.Name);
            Assert.Equal(createdProduct.Price, result.Price);
            Assert.Equal(brand.Name, result.Brand);
            Assert.Equal(category.Name, result.Category);
            _mockProductRepository.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithDuplicateName_ReturnsNull()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var existingProductName = "Existing Product";

            var createDto = new ProductCreateDto
            {
                Name = existingProductName,
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = Guid.NewGuid().ToString(),
                BrandId = Guid.NewGuid().ToString(),
                WebStoreId = webStoreId
            };

            var existingProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = existingProductName,
                Price = 50m,
                Brand = new Brand { Id = Guid.NewGuid(), Name = "Brand" },
                Category = new Category { Id = Guid.NewGuid(), Name = "Category" },
                WebStoreId = webStoreId
            };

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync(new List<Product> { existingProduct });

            // Act
            var result = await _productService.CreateProductAsync(webStoreId, createDto);

            // Assert
            Assert.Null(result);
            _mockProductRepository.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task CreateProductAsync_WhenRepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var createDto = new ProductCreateDto
            {
                Name = "New Product",
                Description = "Test Description",
                Price = 99.99m,
                CategoryId = categoryId.ToString(),
                BrandId = brandId.ToString(),
                WebStoreId = webStoreId
            };

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync(new List<Product>());

            _mockBrandRepository.Setup(r => r.GetBrandByIdAsync(brandId))
                .ReturnsAsync(new Brand { Id = brandId, Name = "Brand" });

            _mockCategoryRepository.Setup(r => r.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(new Category { Id = categoryId, Name = "Category" });

            _mockProductRepository.Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.CreateProductAsync(webStoreId, createDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateProductAsync_WithNoExistingProducts_CreatesNewProduct()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var createDto = new ProductCreateDto
            {
                Name = "New Product",
                Description = "Description",
                Price = 49.99m,
                CategoryId = categoryId.ToString(),
                BrandId = brandId.ToString(),
                WebStoreId = webStoreId
            };

            var brand = new Brand { Id = brandId, Name = "Brand" };
            var category = new Category { Id = categoryId, Name = "Category" };

            var createdProduct = new Product
            {
                Id = productId,
                Name = createDto.Name,
                Price = createDto.Price,
                Brand = brand,
                Category = category,
                WebStoreId = webStoreId
            };

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync(new List<Product>());

            _mockBrandRepository.Setup(r => r.GetBrandByIdAsync(brandId))
                .ReturnsAsync(brand);

            _mockCategoryRepository.Setup(r => r.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(category);

            _mockProductRepository.Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productService.CreateProductAsync(webStoreId, createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            _mockProductRepository.Verify(r => r.CreateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        #endregion

        #region GetAllProductsFromWebStoreAsync Tests

        [Fact]
        public async Task GetAllProductsFromWebStoreAsync_WithExistingProducts_ReturnsProductDtos()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 1",
                    Price = 100m,
                    Brand = new Brand { Id = brandId, Name = "Brand 1" },
                    Category = new Category { Id = categoryId, Name = "Category 1" },
                    WebStoreId = webStoreId
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 2",
                    Price = 200m,
                    Brand = new Brand { Id = brandId, Name = "Brand 2" },
                    Category = new Category { Id = categoryId, Name = "Category 2" },
                    WebStoreId = webStoreId
                }
            };

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsFromWebStoreAsync(webStoreId);

            // Assert
            Assert.NotNull(result);
            var productList = result.ToList();
            Assert.Equal(2, productList.Count);
            Assert.Equal("Product 1", productList[0]?.Name);
            Assert.Equal("Product 2", productList[1]?.Name);
            _mockProductRepository.Verify(r => r.GetAllProductsFromWebStoreAsync(webStoreId), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsFromWebStoreAsync_WithNoProducts_ReturnsEmptyEnumerable()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productService.GetAllProductsFromWebStoreAsync(webStoreId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllProductsFromWebStoreAsync_WithNullResult_ReturnsEmptyEnumerable()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();

            _mockProductRepository.Setup(r => r.GetAllProductsFromWebStoreAsync(webStoreId))
                .ReturnsAsync((IEnumerable<Product>?)null);

            // Act
            var result = await _productService.GetAllProductsFromWebStoreAsync(webStoreId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetProductByIdAsync Tests

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ReturnsProductDto()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m,
                Brand = new Brand { Id = brandId, Name = "Test Brand" },
                Category = new Category { Id = categoryId, Name = "Test Category" },
                Model = "Model X",
                ImageUrl = "https://test.com/image.jpg"
            };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Brand.Name, result.Brand);
            Assert.Equal(product.Category.Name, result.Category);
            _mockProductRepository.Verify(r => r.GetProductByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region UpdateProductAsync Tests

        [Fact]
        public async Task UpdateProductAsync_WithValidInput_ReturnsUpdatedProductDto()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var newBrandId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Old Product",
                Description = "Old Description",
                Price = 50m,
                Brand = new Brand { Id = brandId, Name = "Old Brand" },
                Category = new Category { Id = categoryId, Name = "Old Category" },
                Model = "Model A"
            };

            var updateDto = new ProductUpdateDto
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 99.99m,
                BrandId = newBrandId.ToString(),
                CategoryId = newCategoryId.ToString(),
                Model = "Model B",
                WebStoreId = Guid.NewGuid()
            };

            var newBrand = new Brand { Id = newBrandId, Name = "New Brand" };
            var newCategory = new Category { Id = newCategoryId, Name = "New Category" };

            var updatedProduct = new Product
            {
                Id = productId,
                Name = updateDto.Name,
                Description = updateDto.Description,
                Price = updateDto.Price,
                Brand = newBrand,
                Category = newCategory,
                Model = updateDto.Model
            };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockBrandRepository.Setup(r => r.GetBrandByIdAsync(newBrandId))
                .ReturnsAsync(newBrand);

            _mockCategoryRepository.Setup(r => r.GetCategoryByIdAsync(newCategoryId))
                .ReturnsAsync(newCategory);

            _mockProductRepository.Setup(r => r.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productService.UpdateProductAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Name, result.Name);
            Assert.Equal(updateDto.Price, result.Price);
            _mockProductRepository.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_WithProductNotFound_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var updateDto = new ProductUpdateDto
            {
                Id = productId,
                Name = "Updated Product",
                Price = 99.99m,
                BrandId = Guid.NewGuid().ToString(),
                CategoryId = Guid.NewGuid().ToString(),
                WebStoreId = Guid.NewGuid()
            };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.UpdateProductAsync(updateDto);

            // Assert
            Assert.Null(result);
            _mockProductRepository.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProductAsync_WithBrandNotFound_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var newBrandId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Product",
                Price = 50m,
                Brand = new Brand { Id = brandId, Name = "Brand" },
                Category = new Category { Id = categoryId, Name = "Category" }
            };

            var updateDto = new ProductUpdateDto
            {
                Id = productId,
                Name = "Updated",
                Price = 99.99m,
                BrandId = newBrandId.ToString(),
                CategoryId = newCategoryId.ToString(),
                WebStoreId = Guid.NewGuid()
            };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockBrandRepository.Setup(r => r.GetBrandByIdAsync(newBrandId))
                .ReturnsAsync((Brand?)null);

            // Act
            var result = await _productService.UpdateProductAsync(updateDto);

            // Assert
            Assert.Null(result);
            _mockProductRepository.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProductAsync_WithCategoryNotFound_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var newBrandId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Product",
                Price = 50m,
                Brand = new Brand { Id = brandId, Name = "Brand" },
                Category = new Category { Id = categoryId, Name = "Category" }
            };

            var updateDto = new ProductUpdateDto
            {
                Id = productId,
                Name = "Updated",
                Price = 99.99m,
                BrandId = newBrandId.ToString(),
                CategoryId = newCategoryId.ToString(),
                WebStoreId = Guid.NewGuid()
            };

            var newBrand = new Brand { Id = newBrandId, Name = "New Brand" };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockBrandRepository.Setup(r => r.GetBrandByIdAsync(newBrandId))
                .ReturnsAsync(newBrand);

            _mockCategoryRepository.Setup(r => r.GetCategoryByIdAsync(newCategoryId))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await _productService.UpdateProductAsync(updateDto);

            // Assert
            Assert.Null(result);
            _mockProductRepository.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProductAsync_WhenRepositoryFails_ReturnsNull()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Product",
                Price = 50m,
                Brand = new Brand { Id = brandId, Name = "Brand" },
                Category = new Category { Id = categoryId, Name = "Category" }
            };

            var updateDto = new ProductUpdateDto
            {
                Id = productId,
                Name = "Updated",
                Price = 99.99m,
                BrandId = brandId.ToString(),
                CategoryId = categoryId.ToString(),
                WebStoreId = Guid.NewGuid()
            };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockProductRepository.Setup(r => r.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.UpdateProductAsync(updateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProductAsync_WithSameBrandAndCategory_DoesNotFetchAgain()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var brandId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Product",
                Price = 50m,
                Brand = new Brand { Id = brandId, Name = "Brand" },
                Category = new Category { Id = categoryId, Name = "Category" }
            };

            var updateDto = new ProductUpdateDto
            {
                Id = productId,
                Name = "Updated",
                Price = 99.99m,
                BrandId = brandId.ToString(),
                CategoryId = categoryId.ToString(),
                WebStoreId = Guid.NewGuid()
            };

            var updatedProduct = new Product
            {
                Id = productId,
                Name = updateDto.Name,
                Price = updateDto.Price,
                Brand = existingProduct.Brand,
                Category = existingProduct.Category
            };

            _mockProductRepository.Setup(r => r.GetProductByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockProductRepository.Setup(r => r.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productService.UpdateProductAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            _mockBrandRepository.Verify(r => r.GetBrandByIdAsync(It.IsAny<Guid>()), Times.Never);
            _mockCategoryRepository.Verify(r => r.GetCategoryByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        #endregion

        #region DeleteProductAsync Tests

        [Fact]
        public async Task DeleteProductAsync_WithValidId_CallsRepository()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockProductRepository.Setup(r => r.DeleteProductAsync(productId))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(productId);

            // Assert
            _mockProductRepository.Verify(r => r.DeleteProductAsync(productId), Times.Once);
        }

        #endregion
    }
}
