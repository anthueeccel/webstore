using FluentValidation;

namespace WebStore.API.Features.Product.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("CategoryId must be a valid GUID.");

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("BrandId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("BrandId must be a valid GUID.");

            RuleFor(x => x.WebStoreId)
                .NotEmpty().WithMessage("WebStoreId is required.");
        }
    }
}
