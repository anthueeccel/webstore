using FluentValidation;
using WebStore.API.Shared.Dtos.Commom;

namespace WebStore.API.Shared.Validators
{
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(dto => dto.Street)
                .Length(5, 100).WithMessage("Street must be between 5 and 100 characters.");

            RuleFor(dto => dto.City)
                .Length(3, 50).WithMessage("City must be between 3 and 50 characters.");

            RuleFor(dto => dto.PostalCode)
                .Length(4, 10).WithMessage("Postal Code must be between 4 and 10 characters.")
                .Matches(@"^\d{4}(-\d{4})?$").WithMessage("Zip Code must be a valid format (e.g., 1234 or 1234-56789).");
        }
    }
}
