using WebStore.Domain.Repositories;

namespace WebStore.Application.Dtos.Product
{
    public class ProductCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required string CategoryId { get; set; }
        public required string BrandId { get; set; }
        public string? Model { get; set; }
        public string? ImageUrl { get; set; }
        public required Guid WebStoreId { get; set; }

        public static Domain.Entities.Product? ToEntity(
            IBrandRepository brandRepository,
            ICategoryRepository categoryRepository,
            ProductCreateDto productCreateDto)
        {
            var brand = brandRepository.GetBrandByIdAsync(Guid.Parse(productCreateDto.BrandId))
                .GetAwaiter()
                .GetResult();
            var category = categoryRepository.GetCategoryByIdAsync(Guid.Parse(productCreateDto.CategoryId))
                .GetAwaiter()
                .GetResult();

            if (brand is null)
            {
                return null;
            }
            if (category is null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(productCreateDto.Name) || productCreateDto.Price <= 0)
            {
                return null;
            }
            return new Domain.Entities.Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                Category = category,
                Brand = brand,
                Model = productCreateDto.Model,
                ImageUrl = productCreateDto.ImageUrl,
                WebStoreId = productCreateDto.WebStoreId
            };
        }
    }
}
