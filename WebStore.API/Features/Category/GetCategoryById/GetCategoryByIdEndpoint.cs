using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Category.GetCategoryById
{
    public static class GetCategoryByIdEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/categories/{id}", async (WebStoreDbContext context, ILoggerFactory loggerFactory, Guid id) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetCategoryByIdEndpoint).FullName!);
                var handler = new GetCategoryByIdHandler(context, loggerFactory);
                var response = await handler.HandleAsync(id);
                if (response is null)
                {
                    return Results.NotFound(new ProblemDetails
                    {
                        Title = "Category not found.",
                        Detail = "The requested category does not exist.",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Results.Ok(response);
            })
            .WithName("GetCategoryById")
            ;
        }
    }
}
