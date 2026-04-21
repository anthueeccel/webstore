using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Application.Commands.WebStore
{
    public class UpdateWebStoreCommandHandler(
        ILogger<UpdateWebStoreCommandHandler> logger,
        IWebStoreRepository webStoreRepository) : IRequestHandler<UpdateWebStoreCommand, Guid>
    {
        public async Task<Guid> Handle(UpdateWebStoreCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating the {Name} web store.", request.Name);
            Address? address = null;
            if (request.Address != null)
            {
                address = new Address
                {
                    // Guid.NewGuid() for Address.Id can be handled here or in the Address constructor/factory method
                    City = request.Address.City?.Trim(),
                    Street = request.Address.Street?.Trim(),
                    PostalCode = request.Address.PostalCode?.Trim()
                };
            }

            var webStore = new WebStoreModel
            {
                Name = request.Name,
                Description = request.Description,
                HasDelivery = request.HasDelivery,
                Address = address,
                ContactPhoneNumber = request.ContactPhoneNumber?.Trim(),
                ContactEmail = request.ContactEmail.Trim(),
                ExtraInfo = request.ExtraInfo,
                WebsiteUrl = request.WebsiteUrl?.Trim()
            };

            logger.LogInformation("Saving web store to the repository.");
            var updatedWebStore = await webStoreRepository.UpdateWebStoreAsync(webStore);

            if (updatedWebStore == null)
            {
                logger.LogWarning("Failed to create web store.");
                return Guid.Empty;
            }

            logger.LogInformation("Web store updated successfully with ID {Id}.", webStore.Id);
            return updatedWebStore.Id;
        }
    }
}
