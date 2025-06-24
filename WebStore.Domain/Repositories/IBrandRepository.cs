using WebStore.Domain.Entities;

namespace WebStore.Domain.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandByIdAsync(Guid id);
    }
}
