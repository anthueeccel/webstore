using FluentValidation;

namespace WebStore.API.Features.WebStore.CreateWebStore
{
    public class CreateWebStoreValidator : AbstractValidator<CreateWebStoreRequest>
    {
        public CreateWebStoreValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.ContactEmail)
                .NotEmpty().WithMessage("Contact email is required.")
                .EmailAddress().WithMessage("Contact email must be a valid email address.");

            RuleFor(x => x.ContactPhoneNumber)
                .MaximumLength(50);

            RuleFor(x => x.WebsiteUrl)
                .MaximumLength(200);

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.");
        }
    }
}
