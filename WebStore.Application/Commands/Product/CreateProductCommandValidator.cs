using FluentValidation;

namespace WebStore.Application.Commands.Product
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(obj => obj.Name)
                .Length(3, 100).WithMessage("Product Name must be between 3 and 100 characters.");

            RuleFor(obj => obj.Price)
                .GreaterThan(0).WithMessage("Product Price must be greater than zero.");

            RuleFor(obj => obj.BrandId)
                .NotEmpty().WithMessage("Brand Id is required.")
                .Must(id => Guid.TryParse(id, out var result) && result != Guid.Empty).WithMessage("Brand Id must be a valid GUID.");

            RuleFor(obj => obj.CategoryId)
                .NotEmpty().WithMessage("Category Id is required.")
                .Must(id => Guid.TryParse(id, out var result) && result != Guid.Empty).WithMessage("Category Id must be a valid GUID.");

            RuleFor(obj => obj.ImageUrl)
                .Matches(@"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}(\/[\w\-._~:\/?#\[\]@!$&'()*+,;=%]*)?$")
                .WithMessage("Product Image URL must be a valid URL (ex.: https://your-website.com/product-image.jpg)");
        }
    }
}
