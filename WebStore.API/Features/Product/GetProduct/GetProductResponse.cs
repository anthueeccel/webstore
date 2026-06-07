using WebStore.API.Shared.Dtos.Product;

namespace WebStore.API.Features.Product.GetProduct
{
    public class GetProductResponse
    {
        public required ProductDto Product { get; init; }
    }
}
