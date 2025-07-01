using FluentValidation;
using WebStore.Application.Dtos.WebStore;

namespace WebStore.Application.Dtos.Validators
{
    public class WebStoreCreateDtoValidator : AbstractValidator<WebStoreCreateDto>
    {
        public WebStoreCreateDtoValidator()
        {
            RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Web store Name is required.")
            .Length(3, 100).WithMessage("Web store Name must be between 3 and 100 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Web store Description is required.");

            RuleFor(dto => dto.ContactEmail)
                .NotEmpty().WithMessage("Web store Contact Email is required.")
                .EmailAddress().WithMessage("Web store Contact Email must be a valid email address.");

            RuleFor(dto => dto.WebsiteUrl)
                .Matches(@"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}(\/[\w\-._~:\/?#\[\]@!$&'()*+,;=%]*)?$")
                .WithMessage("Web store Website URL must be a valid URL (ex.: https://your-website.com)");

            RuleFor(dto => dto.ContactPhoneNumber)
                .Matches(@"^(\+?[1-9]\d{1,14}|\d{6,15})$")
                .WithMessage("Contact Phone Number must be a valid phone number. Min: 6 digits (ex.: +123456789123 or 123456");

            RuleFor(dto => dto.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new AddressDtoValidator());
        }
    }
}
