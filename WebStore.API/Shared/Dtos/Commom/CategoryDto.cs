namespace WebStore.API.Shared.Dtos.Commom
{
    public class CategoryDto
    {
        public required Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
