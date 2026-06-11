using WebStore.API.Features.WebStore.GetAllWebStores;
using WebStore.Infrastructure.Persistence;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.Features.WebStore;

[Trait("Category", "Unit")]
public class GetAllWebStoresHandlerTests
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
    public async Task HandleAsync_WhenNoWebStoresExist_ReturnsEmptyList()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new GetAllWebStoresHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetAllWebStoresRequest());

        Assert.NotNull(result);
        Assert.Empty(result.WebStores);
    }

    [Fact]
    public async Task HandleAsync_WhenWebStoresExist_ReturnsAllWebStores()
    {
        var options = CreateUniqueOptions();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.WebStores.AddRange(
                new WebStoreEntity { Name = "Store1", Description = "Desc1", ContactEmail = "a@a.com" },
                new WebStoreEntity { Name = "Store2", Description = "Desc2", ContactEmail = "b@b.com" }
            );
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetAllWebStoresHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetAllWebStoresRequest());

        Assert.NotNull(result);
        Assert.Equal(2, result.WebStores.Count);
        Assert.Contains(result.WebStores, s => s.Name == "Store1");
        Assert.Contains(result.WebStores, s => s.Name == "Store2");
    }
}