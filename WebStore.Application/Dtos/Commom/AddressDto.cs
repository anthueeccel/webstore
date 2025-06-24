using WebStore.Domain.Entities;

namespace WebStore.Application.Dtos.Commom
{
    public class AddressDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }

    public static class AddressDtoExtensions
    {
        public static Address ToEntity(this AddressDto addressDto)
        {
            return new Address
            {
                Id = addressDto.Id,
                City = addressDto.City,
                Street = addressDto.Street,
                PostalCode = addressDto.PostalCode
            };
        }
    }
}
