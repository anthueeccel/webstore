namespace WebStore.Application.Dtos.Product
{
    public record ProductUpdateDto : ProductCreateDto
    {
        public required Guid Id { get; set; }
    }
}
