namespace WebStore.Domain.Entities
{
    public class WebStore : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public bool HasDelivery { get; set; }
        public Address? Address { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public required string ContactEmail { get; set; }
        public string? ExtraInfo { get; set; }
        public List<Product>? Products { get; set; } = [];
    }
}
