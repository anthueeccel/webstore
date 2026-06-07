using WebStore.API.Shared.Dtos.Commom;

namespace WebStore.API.Features.Category.GetAllCategories
{
    public class GetAllCategoriesResponse
    {
        public required List<CategoryDto> Categories { get; init; }
    }
}
