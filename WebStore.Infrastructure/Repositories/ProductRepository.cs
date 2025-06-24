using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Infrastructure.Repositories
{
    internal class ProductRepository(WebStoreDbContext dbContext) : IProductRepository
    {
        public async Task<EntityEntry<Product>> CreateProductAsync(Product product)
        {
            var result = await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> GetAllProductsFromWebStoreAsync(Guid webStoreId)
        {
            var products = await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsNoTracking()
                .Where(p => p.WebStoreId == webStoreId)
                .ToListAsync();
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = await dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);
            return product;
        }

        public async Task<EntityEntry<Product>> UpdateProductAsync(Product product)
        {
            var result = dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync();
            return result;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await GetProductByIdAsync(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }
    }
}
