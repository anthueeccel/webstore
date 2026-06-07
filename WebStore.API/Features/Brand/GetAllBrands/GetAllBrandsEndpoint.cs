using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Brand.GetAllBrands
{
    public static class GetAllBrandsEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/brands", async (WebStoreDbContext context, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetAllBrandsEndpoint).FullName!);
                var handler = new GetAllBrandsHandler(context, loggerFactory);
                var response = await handler.HandleAsync();
                return Results.Ok(response);
            })
            .WithName("GetAllBrands")
            ;
        }
    }
}
