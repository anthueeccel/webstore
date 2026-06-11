using FluentValidation;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Brand.GetBrandById
{
    public static class GetBrandByIdEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/brands/{id}", async (WebStoreDbContext context, IValidator<GetBrandByIdRequest> validator, ILoggerFactory loggerFactory, Guid id) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetBrandByIdEndpoint).FullName!);
                var request = new GetBrandByIdRequest { Id = id };
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("GetBrandById request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new GetBrandByIdHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
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
