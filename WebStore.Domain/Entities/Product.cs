namespace WebStore.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public string? Model { get; set; }
        public string? ImageUrl { get; set; }                
        public Guid WebStoreId { get; set; }
    }
}