using FluentValidation;

namespace WebStore.API.Features.WebStore.GetAllWebStores
{
    public class GetAllWebStoresValidator : AbstractValidator<GetAllWebStoresRequest>
    {
        public GetAllWebStoresValidator()
        {
            // No specific validation required for listing all web stores.
        }
    }
}
