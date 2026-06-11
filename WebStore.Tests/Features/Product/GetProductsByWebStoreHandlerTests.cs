using WebStore.API.Features.Product.GetProductsByWebStore;
using CategoryEntity = WebStore.Domain.Entities.Category;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;
using WebStore.Infrastructure.Persistence;
using ProductEntity = WebStore.Domain.Entities.Product;

namespace WebStore.Tests.Features.Product;

public class GetProductsByWebStoreHandlerTests
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
    public async Task HandleAsync_WhenWebStoreNotFound_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new GetProductsByWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetProductsByWebStoreRequest { WebStoreId = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenWebStoreHasNoProducts_ReturnsEmptyList()
    {
        var options = CreateUniqueOptions();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "EmptyStore",
                Description = "Desc",
                ContactEmail = "e@e.com"
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetProductsByWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetProductsByWebStoreRequest { WebStoreId = webStoreId });

        Assert.NotNull(result);
        Assert.Empty(result.Products);
    }

    [Fact]
    public async Task HandleAsync_WhenWebStoreHasProducts_ReturnsProducts()
    {
        var options = CreateUniqueOptions();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            var category = new CategoryEntity { Name = "Cat1" };
            var brand = new BrandEntity { Name = "Br1" };
            arrangeContext.Categories.Add(category);
            arrangeContext.Brands.Add(brand);
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "Store",
                Description = "Desc",
                ContactEmail = "s@s.com"
            });
            await arrangeContext.SaveChangesAsync();

            arrangeContext.Products.AddRange(
                new ProductEntity { Name = "P1", Price = 10m, Category = category, Brand = brand, WebStoreId = webStoreId },
                new ProductEntity { Name = "P2", Price = 20m, Category = category, Brand = brand, WebStoreId = webStoreId }
            );
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetProductsByWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetProductsByWebStoreRequest { WebStoreId = webStoreId });

        Assert.NotNull(result);
        Assert.Equal(2, result.Products.Count);
        Assert.Contains(result.Products, p => p.Name == "P1");
        Assert.Contains(result.Products, p => p.Name == "P2");
    }
}