namespace WebStore.Application.Dtos.WebStore
{
    public record WebStoreUpdateDto : WebStoreCreateDto
    {
        public required Guid Id { get; init; }
    }
}
