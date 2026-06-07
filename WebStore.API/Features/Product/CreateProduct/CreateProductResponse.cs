using WebStore.API.Shared.Dtos.Product;

namespace WebStore.API.Features.Product.CreateProduct
{
    public class CreateProductResponse
    {
        public required ProductDto Product { get; init; }
    }
}
