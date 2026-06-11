using WebStore.API.Features.WebStore.DeleteWebStore;
using WebStore.Infrastructure.Persistence;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.Features.WebStore;

[Trait("Category", "Unit")]
public class DeleteWebStoreHandlerTests
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
        var handler = new DeleteWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new DeleteWebStoreRequest { Id = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenWebStoreExists_DeletesAndReturnsDeleted()
    {
        var options = CreateUniqueOptions();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "ToDelete",
                Description = "Desc",
                ContactEmail = "del@test.com"
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new DeleteWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new DeleteWebStoreRequest { Id = webStoreId });

        Assert.NotNull(result);
        Assert.True(result.Deleted);

        using var verifyContext = new WebStoreDbContext(options);
        var deletedWebStore = await verifyContext.WebStores.FindAsync(webStoreId);
        Assert.Null(deletedWebStore);
    }
}