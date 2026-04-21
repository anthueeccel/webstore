using WebStore.Application.Dtos.Commom;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Application.Dtos.WebStore
{
    public record WebStoreCreateDto
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public bool HasDelivery { get; init; }
        public AddressDto? Address { get; init; }
        public string? ContactPhoneNumber { get; init; }
        public required string ContactEmail { get; init; }
        public string? ExtraInfo { get; init; }
        public string? WebsiteUrl { get; init; } = null;
        public static WebStoreModel ToEntity(WebStoreCreateDto webStoreCreate) => new()
        {
            Name = webStoreCreate.Name,
            Description = webStoreCreate.Description,
            HasDelivery = webStoreCreate.HasDelivery,
            Address = webStoreCreate.Address?.ToEntity(),
            ContactPhoneNumber = webStoreCreate.ContactPhoneNumber?.Trim(),
            ContactEmail = webStoreCreate.ContactEmail.Trim(),
            ExtraInfo = webStoreCreate.ExtraInfo,
            WebsiteUrl = webStoreCreate.WebsiteUrl?.Trim()
        };
    }
}
