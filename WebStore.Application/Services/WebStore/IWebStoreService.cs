using WebStore.Application.Dtos.WebStore;

namespace WebStore.Application.WebStore.Services
{
    public interface IWebStoreService
    {
        Task<WebStoreDto?> CreateWebStoreAsync(WebStoreCreateDto webStoreCreateDto);
        Task<IEnumerable<WebStoreDto?>> GetAllWebStoresAsync();
        Task<WebStoreDto?> GetWebStoreByIdAsync(Guid id);
    }
}