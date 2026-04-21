using WebStore.Domain.Entities;

namespace WebStore.Application.Dtos.Commom
{
    public record AddressDto
    {
        public Guid Id { get; init; }
        public string City { get; init; }
        public string Street { get; init; }
        public string PostalCode { get; init; }

        public AddressDto(string city, string street, string postalCode)
        {
            Id = Guid.NewGuid();
            City = city;
            Street = street;
            PostalCode = postalCode;
        }
    }

    public static class AddressDtoExtensions
    {
        public static Address ToEntity(this AddressDto addressDto) => new()
        {
            Id = addressDto.Id,
            City = addressDto.City,
            Street = addressDto.Street,
            PostalCode = addressDto.PostalCode
        };
    }
}
