using WebStore.API.Features.Brand.GetAllBrands;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Tests.Features.Brand;

[Trait("Category", "Unit")]
public class GetAllBrandsHandlerTests
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
    public async Task HandleAsync_WhenNoBrandsExist_ReturnsEmptyList()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new GetAllBrandsHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync();

        Assert.NotNull(result);
        Assert.Empty(result.Brands);
    }

    [Fact]
    public async Task HandleAsync_WhenBrandsExist_ReturnsAllBrands()
    {
        var options = CreateUniqueOptions();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.Brands.AddRange(
                new BrandEntity { Name = "Brand1" },
                new BrandEntity { Name = "Brand2" }
            );
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetAllBrandsHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Brands.Count);
        Assert.Contains(result.Brands, b => b.Name == "Brand1");
        Assert.Contains(result.Brands, b => b.Name == "Brand2");
    }
}