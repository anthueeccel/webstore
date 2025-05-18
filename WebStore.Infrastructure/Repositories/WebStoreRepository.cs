using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Repositories;
using WebStore.Infrastructure.Persistence;
using WebstoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Infrastructure.Repositories
{
    internal class WebStoreRepository(WebStoreDbContext dbContext) : IWebStoreRepository
    {
        public async Task<IEnumerable<WebstoreModel>> GetAllWebStoresAsync()
        {
            var webStores = await dbContext.WebStores.ToListAsync();
            return webStores;   
        }

        public async Task<WebstoreModel?> GetWebStoreByIdAsync(Guid id)
        {
            var webStore = await dbContext.WebStores
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
            return webStore;
        }       
    }
}
