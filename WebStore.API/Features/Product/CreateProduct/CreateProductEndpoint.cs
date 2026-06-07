using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Product.CreateProduct
{
    public static class CreateProductEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPost("/api/products", async (WebStoreDbContext context, IValidator<CreateProductRequest> validator, ILoggerFactory loggerFactory, CreateProductRequest request) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(CreateProductEndpoint).FullName!);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("CreateProduct request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new CreateProductHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null)
                {
                    logger.LogError("CreateProduct handler returned null.");
                    return Results.Problem(
                        title: "Unable to create product.",
                        detail: "An unexpected error occurred while creating the product.",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }

                return Results.Created($"/api/products/{response.Product.Id}", response);
            })
            .WithName("CreateProduct");
        }
    }
}
