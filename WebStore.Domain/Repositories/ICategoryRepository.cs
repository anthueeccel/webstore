using WebStore.Domain.Entities;

namespace WebStore.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(Guid id);
    }
}
