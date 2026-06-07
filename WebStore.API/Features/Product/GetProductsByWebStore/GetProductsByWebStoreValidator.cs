using FluentValidation;

namespace WebStore.API.Features.Product.GetProductsByWebStore
{
    public class GetProductsByWebStoreValidator : AbstractValidator<GetProductsByWebStoreRequest>
    {
        public GetProductsByWebStoreValidator()
        {
            RuleFor(x => x.WebStoreId)
                .NotEmpty().WithMessage("WebStoreId is required.");
        }
    }
}
