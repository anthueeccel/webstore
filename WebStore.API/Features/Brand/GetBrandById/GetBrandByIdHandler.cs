using WebStore.API.Shared.Dtos.Commom;
using WebStore.Infrastructure.Persistence;

namespace WebStore.API.Features.Brand.GetBrandById
{
    public class GetBrandByIdHandler
    {
        private readonly WebStoreDbContext _context;
        private readonly ILogger _logger;

        public GetBrandByIdHandler(WebStoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public async Task<GetBrandByIdResponse?> HandleAsync(GetBrandByIdRequest request)
        {
            var brand = await _context.Brands.FindAsync(request.Id);
            if (brand is null)
            {
                _logger.LogWarning("Brand {BrandId} not found.", request.Id);
                return null;
            }

            return new GetBrandByIdResponse
            {
                Brand = new BrandDto { Id = brand.Id, Name = brand.Name }
            };
        }
    }
}