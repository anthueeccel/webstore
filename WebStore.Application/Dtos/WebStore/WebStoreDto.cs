using WebStore.Application.Dtos.Product;
using WebStore.Domain.Entities;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Application.Dtos.WebStore
{
    public class WebStoreDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool HasDelivery { get; set; }
        public Address? Address { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public required string ContactEmail { get; set; }
        public string? ExtraInfo { get; set; }

        public List<ProductDto>? Products { get; set; } = [];

        public static WebStoreDto? FromEntity(WebStoreModel? webStore)
        {
            if (webStore == null)
                return null;

            return new WebStoreDto
            {
                Id = webStore.Id,
                Name = webStore.Name,
                Description = webStore.Description,
                HasDelivery = webStore.HasDelivery,
                Address = webStore.Address,
                ContactPhoneNumber = webStore.ContactPhoneNumber,
                ContactEmail = webStore.ContactEmail,
                ExtraInfo = webStore.ExtraInfo,
                Products = webStore.Products?.Select(ProductDto.FromEntity).ToList()
            };
        }
    }
}
