using WebStore.API.Shared.Dtos.WebStore;

namespace WebStore.API.Features.WebStore.GetAllWebStores
{
    public class GetAllWebStoresResponse
    {
        public required IReadOnlyList<WebStoreDto> WebStores { get; init; }
    }
}
