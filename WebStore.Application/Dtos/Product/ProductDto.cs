using ProductModel = WebStore.Domain.Entities.Product;

namespace WebStore.Application.Dtos.Product
{
    public record ProductDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Name { get; init; }
        public string? Description { get; init; }
        public required decimal Price { get; init; }
        public required string Category { get; init; }
        public required string Brand { get; init; }
        public string? Model { get; set; }
        public string? ImageUrl { get; set; }

        public static ProductDto FromEntity(ProductModel product) => new()
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
