using MediatR;

namespace WebStore.Application.Commands.WebStore
{
    public class UpdateWebStoreCommand : IRequest<Guid>
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool HasDelivery { get; set; }
        public required AddressCommand Address { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public required string ContactEmail { get; set; }
        public string? ExtraInfo { get; set; }
        public string? WebsiteUrl { get; set; }

        public record AddressCommand(
            string? City,
            string? Street,
            string? PostalCode
         );
    }
}
