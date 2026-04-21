using FluentValidation;

namespace WebStore.Application.Commands.WebStore
{
    public class CreateWebStoreValidator : AbstractValidator<CreateWebStoreCommand>
    {
        public CreateWebStoreValidator()
        {
            RuleFor(obj => obj.Name)
                .NotEmpty().WithMessage("Web store Name is required.")
                .Length(3, 100).WithMessage("Web store Name must be between 3 and 100 characters.");

            RuleFor(obj => obj.Description)
                .NotEmpty().WithMessage("Web store Description is required.");

            RuleFor(obj => obj.ContactEmail)
                .NotEmpty().WithMessage("Web store Contact Email is required.")
                .EmailAddress().WithMessage("Web store Contact Email must be a valid email address.");

            RuleFor(obj => obj.WebsiteUrl)
                .Matches(@"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}(\/[\w\-._~:\/?#\[\]@!$&'()*+,;=%]*)?$")
                .WithMessage("Web store Website URL must be a valid URL (ex.: https://your-website.com)");

            RuleFor(obj => obj.ContactPhoneNumber)
                .Matches(@"^(\+?[1-9]\d{1,14}|\d{6,15})$")
                .WithMessage("Contact Phone Number must be a valid phone number. Min: 6 digits (ex.: +123456789123 or 123456");

            RuleFor(obj => obj.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new InlineValidator<CreateWebStoreCommand.AddressCommand>
                {
                        v => v.RuleFor(a => a.City).NotEmpty().WithMessage("City is required."),
                        v => v.RuleFor(a => a.Street).NotEmpty().WithMessage("Street is required."),
                        v => v.RuleFor(a => a.PostalCode).NotEmpty().WithMessage("Postal Code is required.")
                });
        }
    }
}
