using FluentValidation;

namespace WebStore.API.Features.Category.GetCategoryById
{
    public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdRequest>
    {
        public GetCategoryByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}