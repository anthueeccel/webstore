using FluentValidation;

namespace WebStore.API.Features.Brand.GetBrandById
{
    public class GetBrandByIdValidator : AbstractValidator<GetBrandByIdRequest>
    {
        public GetBrandByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}