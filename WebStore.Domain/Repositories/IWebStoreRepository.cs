using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Domain.Repositories
{
    public interface IWebStoreRepository
    {
        Task<IEnumerable<WebStoreModel>> GetAllWebStoresAsync();
        Task<WebStoreModel?> GetWebStoreByIdAsync(Guid id);
        Task<EntityEntry<WebStoreModel>> CreateWebStoreAsync(WebStoreModel webStore);
    }
}
