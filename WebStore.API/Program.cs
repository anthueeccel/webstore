using WebStore.Infrastructure.Extensions;
using WebStore.API.Shared.Extensions;
using WebStore.Infrastructure.Seeders;
using WebStore.API.Features.WebStore.CreateWebStore;
using WebStore.API.Features.WebStore.UpdateWebStore;
using WebStore.API.Features.WebStore.GetWebStore;
using WebStore.API.Features.WebStore.GetAllWebStores;
using WebStore.API.Features.WebStore.DeleteWebStore;
using WebStore.API.Features.Product.CreateProduct;
using WebStore.API.Features.Product.UpdateProduct;
using WebStore.API.Features.Product.GetProduct;
using WebStore.API.Features.Product.GetProductsByWebStore;
using WebStore.API.Features.Product.DeleteProduct;
using WebStore.API.Features.Category.GetAllCategories;
using WebStore.API.Features.Category.GetCategoryById;
using WebStore.API.Features.Brand.GetAllBrands;
using WebStore.API.Features.Brand.GetBrandById;
using WebStore.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Register Infrastructure layer (DbContext, Repositories, Seeder)
builder.Services.AddInfrastructure(builder.Configuration);

// Register API features (validators, handlers, endpoints)
builder.Services.AddApiFeatures(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

// Apply pending migrations and seed the database on startup
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<WebStoreDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IWebStoreSeeder>();
    await seeder.Seed();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while applying migrations or seeding the database.");
    throw;
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Map WebStore endpoints
CreateWebStoreEndpoint.MapEndpoint(app);
UpdateWebStoreEndpoint.MapEndpoint(app);
GetWebStoreEndpoint.MapEndpoint(app);
GetAllWebStoresEndpoint.MapEndpoint(app);
DeleteWebStoreEndpoint.MapEndpoint(app);

// Map Product endpoints
CreateProductEndpoint.MapEndpoint(app);
UpdateProductEndpoint.MapEndpoint(app);
GetProductEndpoint.MapEndpoint(app);
GetProductsByWebStoreEndpoint.MapEndpoint(app);
DeleteProductEndpoint.MapEndpoint(app);

// Map Category endpoints
GetAllCategoriesEndpoint.MapEndpoint(app);
GetCategoryByIdEndpoint.MapEndpoint(app);

// Map Brand endpoints
GetAllBrandsEndpoint.MapEndpoint(app);
GetBrandByIdEndpoint.MapEndpoint(app);

app.Run();

public partial class Program { }
