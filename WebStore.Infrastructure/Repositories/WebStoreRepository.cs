using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Repositories;
using WebStore.Infrastructure.Persistence;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Infrastructure.Repositories
{
    internal class WebStoreRepository(WebStoreDbContext dbContext) : IWebStoreRepository
    {
        public async Task<IEnumerable<WebStoreModel>> GetAllWebStoresAsync()
        {
            var webStores = await dbContext.WebStores
                .Include(ws => ws.Products)
                    .ThenInclude(p => p.Category)
                .Include(ws => ws.Products)
                    .ThenInclude(p => p.Brand)
                .AsNoTracking()
                .ToListAsync();

            return webStores;
        }

        public async Task<WebStoreModel?> GetWebStoreByIdAsync(Guid id)
        {
            var webStore = await dbContext.WebStores
                .Include(ws => ws.Products)
                    .ThenInclude(p => p.Category)
                .Include(ws => ws.Products)
                    .ThenInclude(p => p.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
            return webStore;
        }
    }
}
