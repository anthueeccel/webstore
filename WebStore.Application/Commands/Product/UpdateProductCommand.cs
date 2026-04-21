using MediatR;

namespace WebStore.Application.Commands.Product
{
    public class UpdateProductCommand : IRequest<Guid>
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required string CategoryId { get; set; }
        public required string BrandId { get; set; }
        public string? Model { get; set; }
        public string? ImageUrl { get; set; }
        public required Guid WebStoreId { get; set; }
    }
}
