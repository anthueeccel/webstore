using FluentValidation;

namespace WebStore.API.Features.Product.DeleteProduct
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product id is required.");
        }
    }
}
