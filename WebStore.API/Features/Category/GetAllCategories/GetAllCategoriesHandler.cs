using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Category.GetAllCategories
{
    public class GetAllCategoriesHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetAllCategoriesHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<GetAllCategoriesResponse> HandleAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            _logger.LogInformation("Retrieved {Count} categories.", categories.Count);

            return new GetAllCategoriesResponse
            {
                Categories = categories.Select(category => new CategoryDto { Id = category.Id, Name = category.Name }).ToList()
            };
        }
    }
}