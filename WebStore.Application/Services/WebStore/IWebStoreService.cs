using WebStore.Application.Dtos.WebStore;

namespace WebStore.Application.Services.WebStore
{
    public interface IWebStoreService
    {
        Task<WebStoreDto?> CreateWebStoreAsync(WebStoreCreateDto webStoreCreateDto);
        Task<IEnumerable<WebStoreDto?>> GetAllWebStoresAsync();
        Task<WebStoreDto?> GetWebStoreByIdAsync(Guid id);
        Task<WebStoreDto?> UpdateWebStoreAsync(WebStoreUpdateDto webStoreCreateDto);
        Task DeleteWebStoreAsync(Guid id);
    }
}