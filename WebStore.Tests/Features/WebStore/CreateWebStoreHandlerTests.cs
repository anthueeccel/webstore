using WebStore.API.Features.WebStore.CreateWebStore;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.Features.WebStore;

[Trait("Category", "Unit")]
public class CreateWebStoreHandlerTests
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
    public async Task HandleAsync_CreatesWebStore_ReturnsResponse()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new CreateWebStoreHandler(context, loggerFactory.Object);

        var request = new CreateWebStoreRequest
        {
            Name = "Test Store",
            Description = "Test Description",
            HasDelivery = true,
            ContactEmail = "test@example.com",
            ContactPhoneNumber = "123456789",
            ExtraInfo = "Extra",
            WebsiteUrl = "https://test.com"
        };

        var result = await handler.HandleAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Test Store", result.WebStore.Name);
        Assert.Equal("test@example.com", result.WebStore.ContactEmail);
        Assert.Equal("123456789", result.WebStore.ContactPhoneNumber);
        Assert.Equal("Extra", result.WebStore.ExtraInfo);
        Assert.Equal("https://test.com", result.WebStore.WebsiteUrl);
        Assert.True(result.WebStore.HasDelivery);
        Assert.Null(result.WebStore.Address);
    }

    [Fact]
    public async Task HandleAsync_WithAddress_CreatesWebStoreWithAddress()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new CreateWebStoreHandler(context, loggerFactory.Object);

        var request = new CreateWebStoreRequest
        {
            Name = "Store With Address",
            Description = "Description",
            HasDelivery = false,
            ContactEmail = "store@example.com",
            Address = new AddressDto("City", "Street", "12345")
        };

        var result = await handler.HandleAsync(request);

        Assert.NotNull(result);
        Assert.NotNull(result.WebStore.Address);
        Assert.Equal("City", result.WebStore.Address.City);
        Assert.Equal("Street", result.WebStore.Address.Street);
        Assert.Equal("12345", result.WebStore.Address.PostalCode);
    }

    [Fact]
    public async Task HandleAsync_TrimsContactFields()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new CreateWebStoreHandler(context, loggerFactory.Object);

        var request = new CreateWebStoreRequest
        {
            Name = "Trim Test",
            Description = "Desc",
            ContactEmail = "  email@test.com  ",
            ContactPhoneNumber = "  999999  ",
            WebsiteUrl = "  https://trim.com  "
        };

        var result = await handler.HandleAsync(request);

        Assert.NotNull(result);
        Assert.Equal("email@test.com", result.WebStore.ContactEmail);
        Assert.Equal("999999", result.WebStore.ContactPhoneNumber);
        Assert.Equal("https://trim.com", result.WebStore.WebsiteUrl);
    }
}