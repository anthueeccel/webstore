namespace WebStore.Domain.Entities
{
    public class Brand : BaseEntity
    {       
        public required string Name { get; set; }
        public List<Product>? Products { get; set; } = [];
    }
}