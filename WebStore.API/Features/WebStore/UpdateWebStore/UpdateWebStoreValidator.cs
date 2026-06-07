using FluentValidation;

namespace WebStore.API.Features.WebStore.UpdateWebStore
{
    public class UpdateWebStoreValidator : AbstractValidator<UpdateWebStoreRequest>
    {
        public UpdateWebStoreValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

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
        }
    }
}
