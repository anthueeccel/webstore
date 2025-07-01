using FluentValidation;
using WebStore.Application.Dtos.WebStore;

namespace WebStore.Application.Dtos.Validators
{
    public class WebStoreUpdateDtoValidator : AbstractValidator<WebStoreUpdateDto>
    {
        public WebStoreUpdateDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .Must(id => id != Guid.Empty).WithMessage("Web store Id must be a valid GUID.");

            RuleFor(dto => dto.Name)
                .Length(3, 100).WithMessage("Web store Name must be between 3 and 100 characters.");

            RuleFor(dto => dto.Description)
                .NotEmpty().WithMessage("Web store Description is required.");

            RuleFor(dto => dto.ContactEmail)
                .Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")
                .EmailAddress().WithMessage("Web store Contact Email must be a valid email address.");

            RuleFor(dto => dto.WebsiteUrl)
                .Matches(@"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}(\/[\w\-._~:\/?#\[\]@!$&'()*+,;=%]*)?$")
                .WithMessage("Web store Website URL must be a valid URL (ex.: https://your-website.com)");

            RuleFor(dto => dto.ContactPhoneNumber)
                .Matches(@"^(\+?[1-9]\d{1,14}|\d{6,15})$")
                .WithMessage("Web store Contact Phone Number must be a valid phone number.");
        }
    }
}
