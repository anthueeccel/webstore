using FluentValidation;
using WebStore.Application.Dtos.Product;

namespace WebStore.Application.Dtos.Validators
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .Length(3, 100).WithMessage("Product Name must be between 3 and 100 characters.");
                        
            RuleFor(dto => dto.Price)
                .GreaterThan(0).WithMessage("Product Price must be greater than zero.");

            RuleFor(dto => dto.BrandId)
                .NotEmpty().WithMessage("Brand Id is required.")
                .Must(id => Guid.TryParse(id, out var result) && result != Guid.Empty).WithMessage("Brand Id must be a valid GUID.");

            RuleFor(dto => dto.CategoryId)
                .NotEmpty().WithMessage("Category Id is required.")
                .Must(id => Guid.TryParse(id, out var result) && result != Guid.Empty).WithMessage("Category Id must be a valid GUID.");

            RuleFor(dto => dto.ImageUrl)
                .Matches(@"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}(\/[\w\-._~:\/?#\[\]@!$&'()*+,;=%]*)?$")
                .WithMessage("Product Image URL must be a valid URL (ex.: https://your-website.com/product-image.jpg)");
        }
    }
}
