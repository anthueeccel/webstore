using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Infrastructure.Repositories
{
    internal class BrandRepository(WebStoreDbContext dbContext) : IBrandRepository
    {
        public async Task<Brand?> GetBrandByIdAsync(Guid id)
        {
            var brand = await dbContext.Brands                
                .FirstOrDefaultAsync(w => w.Id == id);
            return brand;
        }
    }
}
