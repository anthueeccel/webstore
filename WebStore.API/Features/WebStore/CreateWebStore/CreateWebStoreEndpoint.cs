using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.CreateWebStore
{
    public static class CreateWebStoreEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPost("/api/webstores", async (WebStoreDbContext context, IValidator<CreateWebStoreRequest> validator, ILoggerFactory loggerFactory, CreateWebStoreRequest request) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(CreateWebStoreEndpoint).FullName!);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("CreateWebStore request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new CreateWebStoreHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null)
                {
                    logger.LogError("CreateWebStore handler returned null.");
                    return Results.Problem(
                        title: "Unable to create WebStore.",
                        detail: "An unexpected error occurred while creating the web store.",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }

                return Results.Created($"/api/webstores/{response.WebStore.Id}", response);
            })
            .WithName("CreateWebStore");
        }
    }
}
