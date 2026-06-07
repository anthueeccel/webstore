using WebStore.API.Shared.Dtos.Product;

namespace WebStore.API.Features.Product.GetProductsByWebStore
{
    public class GetProductsByWebStoreResponse
    {
        public required List<ProductDto> Products { get; init; }
    }
}
