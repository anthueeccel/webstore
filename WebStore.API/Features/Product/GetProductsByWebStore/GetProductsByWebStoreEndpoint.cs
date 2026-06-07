using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.API.Shared.Dtos.Product;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.GetProductsByWebStore
{
    public static class GetProductsByWebStoreEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/webstores/{webStoreId}/products", async (WebStoreDbContext context, IValidator<GetProductsByWebStoreRequest> validator, ILoggerFactory loggerFactory, Guid webStoreId) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetProductsByWebStoreEndpoint).FullName!);
                var request = new GetProductsByWebStoreRequest { WebStoreId = webStoreId };
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("GetProductsByWebStore request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new GetProductsByWebStoreHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null)
                {
                    return Results.NotFound(new ProblemDetails
                    {
                        Title = "Web store not found.",
                        Detail = "The requested web store does not exist.",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Results.Ok(response);
            })
            .WithName("GetProductsByWebStore")
            ;
        }
    }
}
