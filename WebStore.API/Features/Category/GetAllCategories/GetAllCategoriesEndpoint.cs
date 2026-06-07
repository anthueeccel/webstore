using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Category.GetAllCategories
{
    public static class GetAllCategoriesEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/categories", async (WebStoreDbContext context, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetAllCategoriesEndpoint).FullName!);
                var handler = new GetAllCategoriesHandler(context, loggerFactory);
                var response = await handler.HandleAsync();
                return Results.Ok(response);
            })
            .WithName("GetAllCategories")
            ;
        }
    }
}
