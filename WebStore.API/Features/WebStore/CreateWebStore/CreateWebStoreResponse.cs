using WebStore.API.Shared.Dtos.WebStore;

namespace WebStore.API.Features.WebStore.CreateWebStore
{
    public class CreateWebStoreResponse
    {
        public required WebStoreDto WebStore { get; init; }
    }
}
