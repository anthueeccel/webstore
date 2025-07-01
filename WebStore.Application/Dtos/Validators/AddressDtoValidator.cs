using FluentValidation;
using WebStore.Application.Dtos.Commom;

namespace WebStore.Application.Dtos.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(dto => dto.Street)
                .Length(5, 100).WithMessage("Street must be between 5 and 100 characters.");

            RuleFor(dto => dto.City)
                .Length(3, 50).WithMessage("City must be between 3 and 50 characters.");

            RuleFor(dto => dto.PostalCode)
                .Length(5, 10).WithMessage("Postal Code must be between 5 and 10 characters.")
                .Matches(@"^\d{5}(-\d{4})?$").WithMessage("Zip Code must be a valid format (e.g., 12345 or 12345-6789).");
        }
    }
}
