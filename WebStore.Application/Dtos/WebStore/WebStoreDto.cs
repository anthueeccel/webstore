using WebStore.Application.Dtos.Product;
using WebStore.Domain.Entities;

namespace WebStore.Application.Dtos.WebStore
{
    public class WebStoreDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool HasDelivery { get; set; }
        public Address? Address { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public required string ContactEmail { get; set; }
        public string? ExtraInfo { get; set; }
        
        public List<ProductDto>? Products { get; set; } = [];
    }
}
