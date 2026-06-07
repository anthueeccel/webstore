using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.WebStore.GetAllWebStores
{
    public static class GetAllWebStoresEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/webstores", async (WebStoreDbContext context, IValidator<GetAllWebStoresRequest> validator, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetAllWebStoresEndpoint).FullName!);
                var request = new GetAllWebStoresRequest();
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("GetAllWebStores request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new GetAllWebStoresHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
                return Results.Ok(response);
            })
            .WithName("GetAllWebStores")
            ;
        }
    }
}
