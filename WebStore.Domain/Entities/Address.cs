namespace WebStore.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}