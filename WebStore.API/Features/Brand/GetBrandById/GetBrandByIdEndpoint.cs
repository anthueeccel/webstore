using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Brand.GetBrandById
{
    public static class GetBrandByIdEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/brands/{id}", async (WebStoreDbContext context, ILoggerFactory loggerFactory, Guid id) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetBrandByIdEndpoint).FullName!);
                var handler = new GetBrandByIdHandler(context, loggerFactory);
                var response = await handler.HandleAsync(id);
                if (response is null)
                {
                    return Results.NotFound(new ProblemDetails
                    {
                        Title = "Brand not found.",
                        Detail = "The requested brand does not exist.",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Results.Ok(response);
            })
            .WithName("GetBrandById")
            ;
        }
    }
}
