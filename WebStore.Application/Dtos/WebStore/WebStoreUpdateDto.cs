namespace WebStore.Application.Dtos.WebStore
{
    public class WebStoreUpdateDto : WebStoreCreateDto
    {
        public required Guid Id { get; set; }
    }
}
