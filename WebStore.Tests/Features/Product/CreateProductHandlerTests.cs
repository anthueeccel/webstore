using WebStore.API.Features.Product.CreateProduct;
using CategoryEntity = WebStore.Domain.Entities.Category;
using BrandEntity = WebStore.Domain.Entities.Brand;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Tests.Features.Product;

[Trait("Category", "Unit")]
public class CreateProductHandlerTests
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
    public async Task HandleAsync_WhenCategoryIdIsInvalidGuid_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new CreateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new CreateProductRequest
        {
            Name = "Product",
            Price = 10m,
            CategoryId = "invalid-guid",
            BrandId = Guid.NewGuid().ToString(),
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenBrandIdIsInvalidGuid_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new CreateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new CreateProductRequest
        {
            Name = "Product",
            Price = 10m,
            CategoryId = Guid.NewGuid().ToString(),
            BrandId = "invalid-guid",
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenCategoryNotFound_ReturnsNull()
    {
        var options = CreateUniqueOptions();
        var loggerFactory = CreateLoggerFactoryMock();

        using var context = new WebStoreDbContext(options);
        var handler = new CreateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new CreateProductRequest
        {
            Name = "Product",
            Price = 10m,
            CategoryId = Guid.NewGuid().ToString(),
            BrandId = Guid.NewGuid().ToString(),
            WebStoreId = Guid.NewGuid()
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenDependenciesExist_CreatesAndReturnsProduct()
    {
        var options = CreateUniqueOptions();
        var categoryId = Guid.NewGuid();
        var brandId = Guid.NewGuid();
        var webStoreId = Guid.NewGuid();

        using (var arrangeContext = new WebStoreDbContext(options))
        {
            arrangeContext.Categories.Add(new CategoryEntity { Id = categoryId, Name = "Category" });
            arrangeContext.Brands.Add(new BrandEntity { Id = brandId, Name = "Brand" });
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
        var handler = new CreateProductHandler(context, loggerFactory.Object);

        var result = await handler.HandleAsync(new CreateProductRequest
        {
            Name = "NewProduct",
            Description = "New Description",
            Price = 49.99m,
            CategoryId = categoryId.ToString(),
            BrandId = brandId.ToString(),
            WebStoreId = webStoreId,
            Model = "ModelX",
            ImageUrl = "https://img.com/p.png"
        });

        Assert.NotNull(result);
        Assert.Equal("NewProduct", result.Product.Name);
        Assert.Equal("New Description", result.Product.Description);
        Assert.Equal(49.99m, result.Product.Price);
        Assert.Equal("Category", result.Product.Category);
        Assert.Equal("Brand", result.Product.Brand);
        Assert.Equal("ModelX", result.Product.Model);
        Assert.Equal("https://img.com/p.png", result.Product.ImageUrl);

        using var verifyContext = new WebStoreDbContext(options);
        var savedProduct = await verifyContext.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .SingleAsync();
        Assert.Equal("NewProduct", savedProduct.Name);
        Assert.Equal(categoryId, savedProduct.Category.Id);
        Assert.Equal(brandId, savedProduct.Brand.Id);
        Assert.Equal(webStoreId, savedProduct.WebStoreId);
    }
}