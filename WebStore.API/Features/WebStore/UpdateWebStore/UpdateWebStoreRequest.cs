using WebStore.API.Shared.Dtos.Commom;

namespace WebStore.API.Features.WebStore.UpdateWebStore
{
    public class UpdateWebStoreRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool HasDelivery { get; set; }
        public AddressDto? Address { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public required string ContactEmail { get; set; }
        public string? ExtraInfo { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
