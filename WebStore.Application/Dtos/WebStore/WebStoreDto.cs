using WebStore.Application.Dtos.Product;
using WebStore.Domain.Entities;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Application.Dtos.WebStore
{
    public record WebStoreDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public required string Name { get; init; }
        public required string Description { get; init; }
        public bool HasDelivery { get; init; }
        public Address? Address { get; init; }
        public string? ContactPhoneNumber { get; init; }
        public required string ContactEmail { get; init; }
        public string? ExtraInfo { get; init; }
        public string? WebsiteUrl { get; init; }

        public List<ProductDto>? Products { get; set; } = [];

        public static WebStoreDto? FromEntity(WebStoreModel? webStore)
        {
            if (webStore == null)
                return null;

            return new()
            {
                Id = webStore.Id,
                Name = webStore.Name,
                Description = webStore.Description,
                HasDelivery = webStore.HasDelivery,
                Address = webStore.Address,
                ContactPhoneNumber = webStore.ContactPhoneNumber,
                ContactEmail = webStore.ContactEmail,
                ExtraInfo = webStore.ExtraInfo,
                WebsiteUrl = webStore.WebsiteUrl,
                Products = webStore.Products?.Select(ProductDto.FromEntity).ToList()
            };
        }
    }
}
