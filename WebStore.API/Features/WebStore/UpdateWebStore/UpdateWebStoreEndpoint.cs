using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.UpdateWebStore
{
    public static class UpdateWebStoreEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPut("/api/webstores/{id}", async (WebStoreDbContext context, IValidator<UpdateWebStoreRequest> validator, ILoggerFactory loggerFactory, Guid id, UpdateWebStoreRequest request) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(UpdateWebStoreEndpoint).FullName!);
                if (request.Id != id)
                {
                    logger.LogWarning("Route id {RouteId} does not match request id {RequestId}.", id, request.Id);
                    return Results.BadRequest(new ProblemDetails
                    {
                        Title = "Invalid WebStore update request.",
                        Detail = "The request id must match the route id.",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("UpdateWebStore request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new UpdateWebStoreHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                if (response is null)
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
            .WithName("UpdateWebStore")
            ;
        }
    }
}
