using WebStore.Domain.Entities;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Application.Dtos.WebStore
{
    public class WebStoreCreateDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool HasDelivery { get; set; }
        public Address? Address { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public required string ContactEmail { get; set; }
        public string? ExtraInfo { get; set; }
        public string? WebsiteUrl { get; set; }

        public static WebStoreModel ToEntity(WebStoreCreateDto webStoreCreate)
        {
            return new WebStoreModel
            {
                Name = webStoreCreate.Name,
                Description = webStoreCreate.Description,
                HasDelivery = webStoreCreate.HasDelivery,
                Address = webStoreCreate.Address,
                ContactPhoneNumber = webStoreCreate.ContactPhoneNumber,
                ContactEmail = webStoreCreate.ContactEmail,
                ExtraInfo = webStoreCreate.ExtraInfo,
                WebsiteUrl = webStoreCreate.WebsiteUrl                
            };
        }
    }
}
