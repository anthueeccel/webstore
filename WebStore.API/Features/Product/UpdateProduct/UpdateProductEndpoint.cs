using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.API.Shared.Dtos.Product;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.UpdateProduct
{
    public static class UpdateProductEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPut("/api/products/{id}", async (WebStoreDbContext context, IValidator<UpdateProductRequest> validator, ILoggerFactory loggerFactory, Guid id, UpdateProductRequest request) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(UpdateProductEndpoint).FullName!);
                request.Id = id;
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("UpdateProduct request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new UpdateProductHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null)
                {
                    return Results.NotFound(new ProblemDetails
                    {
                        Title = "Product not found.",
                        Detail = "The requested product does not exist or related entity data is invalid.",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            ;
        }
    }
}
