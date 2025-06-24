using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsFromWebStoreAsync(Guid webStoreId);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<EntityEntry<Product>> CreateProductAsync(Product product);
        Task<EntityEntry<Product>> UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
    }
}
