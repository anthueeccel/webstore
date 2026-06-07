using WebStore.API.Shared.Dtos.Commom;

namespace WebStore.API.Features.Brand.GetBrandById
{
    public class GetBrandByIdResponse
    {
        public required BrandDto Brand { get; init; }
    }
}
