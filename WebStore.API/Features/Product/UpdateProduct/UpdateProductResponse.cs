using WebStore.API.Shared.Dtos.Product;

namespace WebStore.API.Features.Product.UpdateProduct
{
    public class UpdateProductResponse
    {
        public required ProductDto Product { get; init; }
    }
}
