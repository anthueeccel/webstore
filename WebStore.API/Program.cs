using WebStore.Infrastructure.Extensions;
using WebStore.Application.Extensions;
using WebStore.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetRequiredService<IWebStoreSeeder>()
    .Seed()
    .Wait();

// Configure the HTTP request pipeline.  

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
