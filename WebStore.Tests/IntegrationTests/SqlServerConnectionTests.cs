using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Tests.IntegrationTests;

[Trait("Category", "Integration")]
public class SqlServerConnectionTests
{
    private static readonly IConfiguration Configuration;

    static SqlServerConnectionTests()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "IntegrationTests"))
            .AddJsonFile("testsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
    }

    private string GetConnectionString()
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");

        // Allow override via environment variable CI/CD
        var envConnectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
        if (!string.IsNullOrEmpty(envConnectionString))
            connectionString = envConnectionString;

        return connectionString ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' not found in configuration.");
    }

    [Fact]
    public async Task OpenSqlConnection_ShouldSucceed()
    {
        // Arrange
        var connectionString = GetConnectionString();
        await using var connection = new SqlConnection(connectionString);

        // Act
        await connection.OpenAsync();

        // Assert
        Assert.Equal(ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task DbContext_CanConnectAsync_ShouldReturnTrue()
    {
        // Arrange
        var connectionString = GetConnectionString();
        var options = new DbContextOptionsBuilder<WebStoreDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        await using var context = new WebStoreDbContext(options);

        // Act
        var canConnect = await context.Database.CanConnectAsync();

        // Assert
        Assert.True(canConnect);
    }
}