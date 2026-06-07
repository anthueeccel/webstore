using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.DeleteProduct
{
    public static class DeleteProductEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapDelete("/api/products/{id}", async (WebStoreDbContext context, IValidator<DeleteProductRequest> validator, ILoggerFactory loggerFactory, Guid id) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(DeleteProductEndpoint).FullName!);
                var request = new DeleteProductRequest { Id = id };
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("DeleteProduct request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new DeleteProductHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null)
                {
                    return Results.NotFound(new ProblemDetails
                    {
                        Title = "Product not found.",
                        Detail = "The requested product does not exist.",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Results.Ok(response);
            })
            .WithName("DeleteProduct")
            ;
        }
    }
}
