using WebStore.API.Features.Product.DeleteProduct;
using CategoryEntity = WebStore.Domain.Entities.Category;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStore.Infrastructure.Persistence;
using ProductEntity = WebStore.Domain.Entities.Product;

namespace WebStore.Tests.Features.Product;

public class DeleteProductHandlerTests
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
        var handler = new DeleteProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new DeleteProductRequest { Id = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenProductExists_DeletesAndReturnsDeleted()
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
                Name = "ToDelete",
                Price = 10m,
                Category = category,
                Brand = brand,
                WebStoreId = Guid.NewGuid()
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new DeleteProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new DeleteProductRequest { Id = productId });

        Assert.NotNull(result);
        Assert.True(result.Deleted);

        using var verifyContext = new WebStoreDbContext(options);
        var deletedProduct = await verifyContext.Products.FindAsync(productId);
        Assert.Null(deletedProduct);
    }
}