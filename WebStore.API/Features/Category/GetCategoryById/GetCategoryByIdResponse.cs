using WebStore.API.Shared.Dtos.Commom;

namespace WebStore.API.Features.Category.GetCategoryById
{
    public class GetCategoryByIdResponse
    {
        public required CategoryDto Category { get; init; }
    }
}
