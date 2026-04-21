using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;
using WebStoreModel = WebStore.Domain.Entities.WebStore;

namespace WebStore.Application.Commands.WebStore
{
    public class CreateWebStoreCommandHandler(
        ILogger<CreateWebStoreCommandHandler> logger,
        IWebStoreRepository webStoreRepository) : IRequestHandler<CreateWebStoreCommand, Guid>
    {
        public async Task<Guid> Handle(CreateWebStoreCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating a new web store with name {Name}.", request.Name);
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
            var createdWebStore = await webStoreRepository.CreateWebStoreAsync(webStore);

            if (createdWebStore == null)
            {
                logger.LogWarning("Failed to create web store.");
                return Guid.Empty;
            }

            logger.LogInformation("Web store created successfully with ID {Id}.", webStore.Id);
            return createdWebStore.Id;
        }
    }
}
