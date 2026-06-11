using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebStore.API.Features.WebStore.GetAllWebStores;
using WebStore.Infrastructure.Persistence;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.IntegrationTests;

[Trait("Category", "E2E")]
public class GetAllWebStoresEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetAllWebStoresEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_GetAllWebStores_ShouldContainSeededStore()
    {
        // Arrange
        var uniqueStoreName = $"E2E_Test_Store_{Guid.NewGuid()}";

        // Seed a known store directly into the real database via DbContext
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WebStoreDbContext>();
            var seededStore = new WebStoreEntity
            {
                Name = uniqueStoreName,
                Description = "End-to-end test store",
                ContactEmail = "e2e@test.com"
            };
            context.WebStores.Add(seededStore);
            await context.SaveChangesAsync();
        }

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/webstores");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetAllWebStoresResponse>();

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result.WebStores, s => s.Name == uniqueStoreName);

        // Cleanup: remove the seeded record from the database
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<WebStoreDbContext>();
            var storeToRemove = await context.WebStores
                .FirstOrDefaultAsync(s => s.Name == uniqueStoreName);
            if (storeToRemove is not null)
            {
                context.WebStores.Remove(storeToRemove);
                await context.SaveChangesAsync();
            }
        }
    }
}