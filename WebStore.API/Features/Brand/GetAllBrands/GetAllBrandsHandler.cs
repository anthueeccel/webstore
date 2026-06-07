using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Brand.GetAllBrands
{
    public class GetAllBrandsHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetAllBrandsHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<GetAllBrandsResponse> HandleAsync()
        {
            var brands = await _context.Brands.ToListAsync();
            _logger.LogInformation("Retrieved {Count} brands.", brands.Count);

            return new GetAllBrandsResponse
            {
                Brands = brands.Select(brand => new BrandDto { Id = brand.Id, Name = brand.Name }).ToList()
            };
        }
    }
}