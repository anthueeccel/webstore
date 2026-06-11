using WebStore.API.Features.Product.UpdateProduct;
using CategoryEntity = WebStore.Domain.Entities.Category;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;
using WebStore.Infrastructure.Persistence;
using ProductEntity = WebStore.Domain.Entities.Product;

namespace WebStore.Tests.Features.Product;

public class UpdateProductHandlerTests
{
    private static Mock<ILoggerFactory> CreateLoggerFactoryMock()
    {
        var loggerMock = new Mock<ILogger>();
        var loggerFactoryMock = new Mock<ILoggerFactory>();
        loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
        return loggerFactoryMock;
    }

    private static DbContextOptions<WebStoreDbContext> CreateUniqueOptions()
    {
        return new DbContextOptionsBuilder<WebStoreDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;
    }

    [Fact]
    public async Task HandleAsync_WhenProductNotFound_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new UpdateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateProductRequest
        {
            Id = Guid.NewGuid(),
            Name = "Updated",
            Price = 10m,
            CategoryId = Guid.NewGuid().ToString(),
            BrandId = Guid.NewGuid().ToString(),
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenCategoryIdIsInvalidGuid_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var productId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            var category = new CategoryEntity { Name = "Cat" };
            var brand = new BrandEntity { Name = "Br" };
            arrangeContext.Categories.Add(category);
            arrangeContext.Brands.Add(brand);
            await arrangeContext.SaveChangesAsync();

            arrangeContext.Products.Add(new ProductEntity
            {
                Id = productId,
                Name = "Original",
                Price = 5m,
                Category = category,
                Brand = brand,
                WebStoreId = Guid.NewGuid()
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new UpdateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateProductRequest
        {
            Id = productId,
            Name = "Updated",
            Price = 10m,
            CategoryId = "invalid-guid",
            BrandId = Guid.NewGuid().ToString(),
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenBrandIdIsInvalidGuid_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var productId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            var category = new CategoryEntity { Name = "Cat" };
            var brand = new BrandEntity { Name = "Br" };
            arrangeContext.Categories.Add(category);
            arrangeContext.Brands.Add(brand);
            await arrangeContext.SaveChangesAsync();

            arrangeContext.Products.Add(new ProductEntity
            {
                Id = productId,
                Name = "Original",
                Price = 5m,
                Category = category,
                Brand = brand,
                WebStoreId = Guid.NewGuid()
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new UpdateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateProductRequest
        {
            Id = productId,
            Name = "Updated",
            Price = 10m,
            CategoryId = Guid.NewGuid().ToString(),
            BrandId = "invalid-guid",
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenDependencyNotFound_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var productId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            var category = new CategoryEntity { Name = "Cat" };
            var brand = new BrandEntity { Name = "Br" };
            arrangeContext.Categories.Add(category);
            arrangeContext.Brands.Add(brand);
            await arrangeContext.SaveChangesAsync();

            arrangeContext.Products.Add(new ProductEntity
            {
                Id = productId,
                Name = "Original",
                Price = 5m,
                Category = category,
                Brand = brand,
                WebStoreId = Guid.NewGuid()
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new UpdateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateProductRequest
        {
            Id = productId,
            Name = "Updated",
            Price = 10m,
            CategoryId = Guid.NewGuid().ToString(),
            BrandId = Guid.NewGuid().ToString(),
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenValidRequest_UpdatesAndReturnsProduct()
    {
        var options = CreateUniqueOptions();
        var productId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            var originalCategory = new CategoryEntity { Id = Guid.NewGuid(), Name = "OriginalCat" };
            var originalBrand = new BrandEntity { Id = Guid.NewGuid(), Name = "OriginalBr" };
            arrangeContext.Categories.Add(originalCategory);
            arrangeContext.Brands.Add(originalBrand);

            var newCategory = new CategoryEntity { Id = categoryId, Name = "NewCategory" };
            var newBrand = new BrandEntity { Id = brandId, Name = "NewBrand" };
            arrangeContext.Categories.Add(newCategory);
            arrangeContext.Brands.Add(newBrand);
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "Store",
                Description = "Desc",
                ContactEmail = "s@s.com"
            });
            await arrangeContext.SaveChangesAsync();

            arrangeContext.Products.Add(new ProductEntity
            {
                Id = productId,
                Name = "Original",
                Price = 5m,
                Category = originalCategory,
                Brand = originalBrand,
                WebStoreId = Guid.NewGuid()
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new UpdateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateProductRequest
        {
            Id = productId,
            Name = "UpdatedProduct",
            Description = "Updated Desc",
            Price = 99.99m,
            CategoryId = categoryId.ToString(),
            BrandId = brandId.ToString(),
            WebStoreId = webStoreId,
            Model = "NewModel",
            ImageUrl = "https://img.com/new.png"
        });

        Assert.NotNull(result);
        Assert.Equal("UpdatedProduct", result.Product.Name);
        Assert.Equal("Updated Desc", result.Product.Description);
        Assert.Equal(99.99m, result.Product.Price);
        Assert.Equal("NewCategory", result.Product.Category);
        Assert.Equal("NewBrand", result.Product.Brand);
        Assert.Equal("NewModel", result.Product.Model);
        Assert.Equal("https://img.com/new.png", result.Product.ImageUrl);
    }
}