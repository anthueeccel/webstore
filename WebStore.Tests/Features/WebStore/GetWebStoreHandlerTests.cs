using WebStore.API.Features.WebStore.GetWebStore;
using WebStore.Infrastructure.Persistence;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.Features.WebStore;

[Trait("Category", "Unit")]
public class GetWebStoreHandlerTests
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
        var handler = new GetWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetWebStoreRequest { Id = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenWebStoreExists_ReturnsWebStore()
    {
        var options = CreateUniqueOptions();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "TestStore",
                Description = "Test Description",
                ContactEmail = "test@example.com",
                HasDelivery = true
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetWebStoreRequest { Id = webStoreId });

        Assert.NotNull(result);
        Assert.NotNull(result.WebStore);
        Assert.Equal(webStoreId, result.WebStore.Id);
        Assert.Equal("TestStore", result.WebStore.Name);
        Assert.Equal("test@example.com", result.WebStore.ContactEmail);
    }
}