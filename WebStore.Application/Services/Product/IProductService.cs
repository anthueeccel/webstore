using WebStore.Application.Dtos.Product;

namespace WebStore.Application.Services.Product
{
    public interface IProductService
    {
        Task<ProductDto?> CreateProductAsync(Guid webStoreId, ProductCreateDto productCreateDto);
        Task<IEnumerable<ProductDto?>> GetAllProductsFromWebStoreAsync(Guid webStoreId);
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<ProductDto?> UpdateProductAsync(ProductUpdateDto productCreateDto);
        Task DeleteProductAsync(Guid id);
    }
}
