using Microsoft.Extensions.Logging;
using Moq;
using WebStore.Application.Dtos.Commom;
using WebStore.Application.Dtos.WebStore;
using WebStore.Application.Services.WebStore;
using WebStore.Domain.Entities;
using WebStore.Domain.Repositories;
using WebStoreEntity = WebStore.Domain.Entities.WebStore;

namespace WebStore.Tests.Services
{
    public class WebStoreServiceTests
    {
        private readonly Mock<IWebStoreRepository> _mockWebStoreRepository;
        private readonly Mock<ILogger<WebStoreService>> _mockLogger;
        private readonly WebStoreService _webStoreService;

        public WebStoreServiceTests()
        {
            _mockWebStoreRepository = new Mock<IWebStoreRepository>();
            _mockLogger = new Mock<ILogger<WebStoreService>>();
            _webStoreService = new WebStoreService(_mockWebStoreRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateWebStoreAsync_WithValidInput_ReturnsCreatedWebStore()
        {
            // Arrange
            var createDto = new WebStoreCreateDto
            {
                Name = "Test WebStore",
                Description = "Test Description",
                HasDelivery = true,
                ContactEmail = "contact@test.com",
                ContactPhoneNumber = "123456789",
                WebsiteUrl = "https://test.com"
            };

            var createdWebStore = new WebStoreEntity
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Description = createDto.Description,
                HasDelivery = createDto.HasDelivery,
                ContactEmail = createDto.ContactEmail,
                ContactPhoneNumber = createDto.ContactPhoneNumber,
                WebsiteUrl = createDto.WebsiteUrl
            };

            _mockWebStoreRepository.Setup(r => r.CreateWebStoreAsync(It.IsAny<WebStoreEntity>()))
                .ReturnsAsync(createdWebStore);


            // Act
            var result = await _webStoreService.CreateWebStoreAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createDto.Name, result.Name);
            Assert.Equal(createDto.Description, result.Description);
            _mockWebStoreRepository.Verify(r => r.CreateWebStoreAsync(It.IsAny<WebStoreEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateWebStoreAsync_WithRepositoryFailure_ReturnsNull()
        {
            // Arrange
            var createDto = new WebStoreCreateDto
            {
                Name = "Test WebStore",
                Description = "Test Description",
                ContactEmail = "test@test.com"
            };

            _mockWebStoreRepository.Setup(r => r.CreateWebStoreAsync(It.IsAny<WebStoreEntity>()))
                .ReturnsAsync((WebStoreEntity?)null);

            // Act
            var result = await _webStoreService.CreateWebStoreAsync(createDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetWebStoreByIdAsync_WithValidId_ReturnsWebStore()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var webStore = new WebStoreEntity
            {
                Id = webStoreId,
                Name = "Test WebStore",
                Description = "Test Description",
                ContactEmail = "contact@test.com"
            };

            _mockWebStoreRepository.Setup(r => r.GetWebStoreByIdAsync(webStoreId))
                .ReturnsAsync(webStore);

            // Act
            var result = await _webStoreService.GetWebStoreByIdAsync(webStoreId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(webStoreId, result.Id);
            Assert.Equal(webStore.Name, result.Name);
            _mockWebStoreRepository.Verify(r => r.GetWebStoreByIdAsync(webStoreId), Times.Once);
        }

        [Fact]
        public async Task GetWebStoreByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            _mockWebStoreRepository.Setup(r => r.GetWebStoreByIdAsync(webStoreId))
                .ReturnsAsync((WebStoreEntity?)null);

            // Act
            var result = await _webStoreService.GetWebStoreByIdAsync(webStoreId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllWebStoresAsync_WithWebStores_ReturnsWebStoreList()
        {
            // Arrange
            var webStores = new List<WebStoreEntity>
            {
                new WebStoreEntity { Id = Guid.NewGuid(), Name = "WebStore 1", Description = "Description 1", ContactEmail = "email1@test.com" },
                new WebStoreEntity { Id = Guid.NewGuid(), Name = "WebStore 2", Description = "Description 2", ContactEmail = "email2@test.com" }
            };

            _mockWebStoreRepository.Setup(r => r.GetAllWebStoresAsync())
                .ReturnsAsync(webStores);

            // Act
            var result = await _webStoreService.GetAllWebStoresAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockWebStoreRepository.Verify(r => r.GetAllWebStoresAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllWebStoresAsync_WithNoWebStores_ReturnsEmptyList()
        {
            // Arrange
            _mockWebStoreRepository.Setup(r => r.GetAllWebStoresAsync())
                .ReturnsAsync(new List<WebStoreEntity>());

            // Act
            var result = await _webStoreService.GetAllWebStoresAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateWebStoreAsync_WithValidInput_ReturnsUpdatedWebStore()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var existingWebStore = new WebStoreEntity
            {
                Id = webStoreId,
                Name = "Old Name",
                Description = "Old Description",
                ContactEmail = "old@test.com"
            };

            var updateDto = new WebStoreUpdateDto
            {
                Id = webStoreId,
                Name = "New Name",
                Description = "New Description",
                ContactEmail = "new@test.com",
                ContactPhoneNumber = "987654321",
                HasDelivery = true,
                WebsiteUrl = "https://new.com"
            };

            _mockWebStoreRepository.Setup(r => r.GetWebStoreByIdAsync(webStoreId))
                .ReturnsAsync(existingWebStore);

            _mockWebStoreRepository.Setup(r => r.UpdateWebStoreAsync(It.IsAny<WebStoreEntity>()))
                .ReturnsAsync(existingWebStore);

            // Act
            var result = await _webStoreService.UpdateWebStoreAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            _mockWebStoreRepository.Verify(r => r.UpdateWebStoreAsync(It.IsAny<WebStoreEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateWebStoreAsync_WithNonExistentWebStore_ReturnsNull()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            var updateDto = new WebStoreUpdateDto
            {
                Id = webStoreId,
                Name = "New Name",
                Description = "Description",
                ContactEmail = "email@test.com"
            };

            _mockWebStoreRepository.Setup(r => r.GetWebStoreByIdAsync(webStoreId))
                .ReturnsAsync((WebStoreEntity?)null);

            // Act
            var result = await _webStoreService.UpdateWebStoreAsync(updateDto);

            // Assert
            Assert.Null(result);
            _mockWebStoreRepository.Verify(r => r.UpdateWebStoreAsync(It.IsAny<WebStoreEntity>()), Times.Never);
        }

        [Fact]
        public async Task DeleteWebStoreAsync_WithValidId_CallsRepository()
        {
            // Arrange
            var webStoreId = Guid.NewGuid();
            _mockWebStoreRepository.Setup(r => r.DeleteWebStoreAsync(webStoreId))
                .Returns(Task.CompletedTask);

            // Act
            await _webStoreService.DeleteWebStoreAsync(webStoreId);

            // Assert
            _mockWebStoreRepository.Verify(r => r.DeleteWebStoreAsync(webStoreId), Times.Once);
        }
    }
}
