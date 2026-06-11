using WebStore.API.Features.Product.GetProduct;
using CategoryEntity = WebStore.Domain.Entities.Category;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStore.Infrastructure.Persistence;
using ProductEntity = WebStore.Domain.Entities.Product;

namespace WebStore.Tests.Features.Product;

public class GetProductHandlerTests
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
        var handler = new GetProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetProductRequest { Id = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenProductExists_ReturnsProduct()
    {
        var options = CreateUniqueOptions();
        var productId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            var category = new CategoryEntity { Name = "TestCategory" };
            var brand = new BrandEntity { Name = "TestBrand" };
            arrangeContext.Categories.Add(category);
            arrangeContext.Brands.Add(brand);
            await arrangeContext.SaveChangesAsync();

            arrangeContext.Products.Add(new ProductEntity
            {
                Id = productId,
                Name = "TestProduct",
                Price = 99.99m,
                Category = category,
                Brand = brand,
                WebStoreId = Guid.NewGuid()
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetProductRequest { Id = productId });

        Assert.NotNull(result);
        Assert.Equal(productId, result.Product.Id);
        Assert.Equal("TestProduct", result.Product.Name);
        Assert.Equal(99.99m, result.Product.Price);
        Assert.Equal("TestCategory", result.Product.Category);
        Assert.Equal("TestBrand", result.Product.Brand);
    }
}