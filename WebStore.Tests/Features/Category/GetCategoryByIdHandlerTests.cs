using WebStore.API.Features.Category.GetCategoryById;
using CategoryEntity = WebStore.Domain.Entities.Category;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Tests.Features.Category;

[Trait("Category", "Unit")]
public class GetCategoryByIdHandlerTests
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
    public async Task HandleAsync_WhenCategoryNotFound_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new GetCategoryByIdHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetCategoryByIdRequest { Id = Guid.NewGuid() });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenCategoryExists_ReturnsCategory()
    {
        var options = CreateUniqueOptions();
        var categoryId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.Categories.Add(new CategoryEntity { Id = categoryId, Name = "TestCategory" });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetCategoryByIdHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new GetCategoryByIdRequest { Id = categoryId });

        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Category.Id);
        Assert.Equal("TestCategory", result.Category.Name);
    }
}