using WebStore.API.Shared.Dtos.WebStore;

namespace WebStore.API.Features.WebStore.UpdateWebStore
{
    public class UpdateWebStoreResponse
    {
        public required WebStoreDto WebStore { get; init; }
    }
}
