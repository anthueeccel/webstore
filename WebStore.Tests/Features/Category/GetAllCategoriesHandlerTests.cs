using WebStore.API.Features.Category.GetAllCategories;
using CategoryEntity = WebStore.Domain.Entities.Category;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Tests.Features.Category;

[Trait("Category", "Unit")]
public class GetAllCategoriesHandlerTests
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
    public async Task HandleAsync_WhenNoCategoriesExist_ReturnsEmptyList()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new GetAllCategoriesHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync();

        Assert.NotNull(result);
        Assert.Empty(result.Categories);
    }

    [Fact]
    public async Task HandleAsync_WhenCategoriesExist_ReturnsAllCategories()
    {
        var options = CreateUniqueOptions();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.Categories.AddRange(
                new CategoryEntity { Name = "Category1" },
                new CategoryEntity { Name = "Category2" }
            );
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new GetAllCategoriesHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Categories.Count);
        Assert.Contains(result.Categories, c => c.Name == "Category1");
        Assert.Contains(result.Categories, c => c.Name == "Category2");
    }
}