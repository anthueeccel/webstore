using WebStore.Infrastructure.Extensions;
using WebStore.Application.Extensions;
using WebStore.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetRequiredService<IWebStoreSeeder>()
    .Seed()
    .Wait();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
