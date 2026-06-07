using FluentValidation;

namespace WebStore.API.Features.WebStore.GetWebStore
{
    public class GetWebStoreValidator : AbstractValidator<GetWebStoreRequest>
    {
        public GetWebStoreValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
