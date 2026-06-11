using FluentValidation;

using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Category.GetCategoryById
{
    public static class GetCategoryByIdEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapGet("/api/categories/{id}", async (WebStoreDbContext context, IValidator<GetCategoryByIdRequest> validator, ILoggerFactory loggerFactory, Guid id) =>
            {
                var logger = loggerFactory.CreateLogger(typeof(GetCategoryByIdEndpoint).FullName!);
                var request = new GetCategoryByIdRequest { Id = id };
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    logger.LogWarning("GetCategoryById request validation failed.");
                    var errors = validationResult.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var handler = new GetCategoryByIdHandler(context, loggerFactory);
                var response = await handler.HandleAsync(request);
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
