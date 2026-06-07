using WebStore.API.Shared.Dtos.Commom;

namespace WebStore.API.Features.Brand.GetAllBrands
{
    public class GetAllBrandsResponse
    {
        public required List<BrandDto> Brands { get; init; }
    }
}
