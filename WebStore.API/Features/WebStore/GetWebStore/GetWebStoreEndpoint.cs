using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.GetWebStore
{
    public static class GetWebStoreEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/webstores/{id}", async (WebStoreDbContext context, IValidator<GetWebStoreRequest> validator, ILoggerFactory loggerFactory, Guid id) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetWebStoreEndpoint).FullName!);
                var request = new GetWebStoreRequest { Id = id };
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("GetWebStore request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new GetWebStoreHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null || response.WebStore is null)
                {
                    return Results.NotFound(new ProblemDetails
                    {
                        Title = "WebStore not found.",
                        Detail = "The requested web store does not exist.",
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Results.Ok(response);
            })
            .WithName("GetWebStore")
            ;
        }
    }
}
