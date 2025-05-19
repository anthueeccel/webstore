namespace WebStore.Application.Dtos.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Category { get; set; }
        public required string Brand { get; set; }
        public string? ImageUrl { get; set; }        
    }
}
