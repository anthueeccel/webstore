using WebStore.API.Features.WebStore.UpdateWebStore;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.Features.WebStore;

[Trait("Category", "Unit")]
public class UpdateWebStoreHandlerTests
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
        var handler = new UpdateWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateWebStoreRequest
        {
            Id = Guid.NewGuid(),
            Name = "Updated",
            Description = "Updated Desc",
            ContactEmail = "u@u.com"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenWebStoreExists_UpdatesAndReturns()
    {
        var options = CreateUniqueOptions();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "Original",
                Description = "Original Desc",
                ContactEmail = "original@test.com",
                HasDelivery = false
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new UpdateWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateWebStoreRequest
        {
            Id = webStoreId,
            Name = "Updated Store",
            Description = "Updated Description",
            HasDelivery = true,
            ContactEmail = "  updated@test.com  ",
            ContactPhoneNumber = "  555-1234  ",
            WebsiteUrl = "  https://updated.com  ",
            ExtraInfo = "New Extra Info"
        });

        Assert.NotNull(result);
        Assert.Equal("Updated Store", result.WebStore.Name);
        Assert.Equal("Updated Description", result.WebStore.Description);
        Assert.True(result.WebStore.HasDelivery);
        Assert.Equal("updated@test.com", result.WebStore.ContactEmail);
        Assert.Equal("555-1234", result.WebStore.ContactPhoneNumber);
        Assert.Equal("https://updated.com", result.WebStore.WebsiteUrl);
        Assert.Equal("New Extra Info", result.WebStore.ExtraInfo);
    }

    [Fact]
    public async Task HandleAsync_WithAddress_UpdatesAddress()
    {
        var options = CreateUniqueOptions();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.WebStores.Add(new WebStoreEntity
            {
                Id = webStoreId,
                Name = "Store",
                Description = "Desc",
                ContactEmail = "s@s.com"
            });
            await arrangeContext.SaveChangesAsync();
        }

        var loggerFactory = CreateLoggerFactoryMock();
        using var context = new WebStoreDbContext(options);
        var handler = new UpdateWebStoreHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new UpdateWebStoreRequest
        {
            Id = webStoreId,
            Name = "Store",
            Description = "Desc",
            ContactEmail = "s@s.com",
            Address = new AddressDto("NewCity", "NewStreet", "54321")
        });

        Assert.NotNull(result);
        Assert.NotNull(result.WebStore.Address);
        Assert.Equal("NewCity", result.WebStore.Address.City);
        Assert.Equal("NewStreet", result.WebStore.Address.Street);
        Assert.Equal("54321", result.WebStore.Address.PostalCode);
    }
}