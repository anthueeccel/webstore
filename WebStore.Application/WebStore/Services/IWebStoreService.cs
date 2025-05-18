namespace WebStore.Application.WebStore.Services
{
    public interface IWebStoreService
    {
        Task<IEnumerable<Domain.Entities.WebStore>> GetAllWebStoresAsync();
        Task<Domain.Entities.WebStore> GetWebStoreByIdAsync(Guid id);
    }
}