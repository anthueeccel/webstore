using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Category.GetCategoryById
{
    public class GetCategoryByIdHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetCategoryByIdHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<GetCategoryByIdResponse?> HandleAsync(GetCategoryByIdRequest request)
        {
            var category = await _context.Categories.FindAsync(request.Id);
            if (category is null)
            {
                _logger.LogWarning("Category {CategoryId} not found.", request.Id);
                return null;
            }

            return new GetCategoryByIdResponse
            {
                Category = new CategoryDto { Id = category.Id, Name = category.Name }
            };
        }
    }
}