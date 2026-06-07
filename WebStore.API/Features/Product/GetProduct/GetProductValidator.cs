using FluentValidation;

namespace WebStore.API.Features.Product.GetProduct
{
    public class GetProductValidator : AbstractValidator<GetProductRequest>
    {
        public GetProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product id is required.");
        }
    }
}
