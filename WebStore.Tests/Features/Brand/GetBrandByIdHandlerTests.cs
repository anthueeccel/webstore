using WebStore.API.Features.Brand.GetBrandById;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Tests.Features.Brand;

[Trait("Category", "Unit")]
public class GetBrandByIdHandlerTests
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
    public async Task HandleAsync_WhenBrandNotFound_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new GetBrandByIdHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetBrandByIdRequest { Id = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenBrandExists_ReturnsBrand()
    {
        var options = CreateUniqueOptions();
        var brandId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.Brands.Add(new BrandEntity { Id = brandId, Name = "TestBrand" });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetBrandByIdHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetBrandByIdRequest { Id = brandId });

        Assert.NotNull(result);
        Assert.Equal(brandId, result.Brand.Id);
        Assert.Equal("TestBrand", result.Brand.Name);
    }
}