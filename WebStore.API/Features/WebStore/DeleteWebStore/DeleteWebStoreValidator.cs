using FluentValidation;

namespace WebStore.API.Features.WebStore.DeleteWebStore
{
    public class DeleteWebStoreValidator : AbstractValidator<DeleteWebStoreRequest>
    {
        public DeleteWebStoreValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
