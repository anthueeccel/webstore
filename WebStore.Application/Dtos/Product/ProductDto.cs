using ProductModel = WebStore.Domain.Entities.Product;

namespace WebStore.Application.Dtos.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required string Category { get; set; }
        public required string Brand { get; set; }
        public string? Model { get; set; }
        public string? ImageUrl { get; set; }

        public static ProductDto FromEntity(ProductModel product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product?.Description,
                Price = product!.Price,
                Category = product.Category.Name,
                Brand = product.Brand.Name,
                ImageUrl = product?.ImageUrl,
                Model = product?.Model
            };
        }
    }
}
