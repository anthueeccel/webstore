namespace WebStore.Application.Dtos.Product
{
    public class ProductUpdateDto : ProductCreateDto
    {
        public required Guid Id { get; set; }
    }
}
