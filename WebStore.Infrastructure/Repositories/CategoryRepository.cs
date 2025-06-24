using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Infrastructure.Repositories
{
    internal class CategoryRepository(WebStoreDbContext dbContext) : ICategoryRepository
    {
        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            var category = await dbContext.Categories
                .FirstOrDefaultAsync(w => w.Id == id);
            return category;
        }
    }
}
