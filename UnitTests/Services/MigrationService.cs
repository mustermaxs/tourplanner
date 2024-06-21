using NUnit.Framework;
using Moq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tourplanner.Entities.Tours;
using Tourplanner.Repositories;
using Tourplanner.Services;
using Tourplanner.Exceptions;

namespace UnitTests.Services;

[TestFixture]
    public class MigrationServiceTests
    {
        private Mock<ITourRepository> _tourRepositoryMock;
        private MigrationService _migrationService;

        [SetUp]
        public void SetUp()
        {
            _tourRepositoryMock = new Mock<ITourRepository>();
            _migrationService = new MigrationService(_tourRepositoryMock.Object);
        }

        [Test]
        public async Task ExportTour_ValidTourId_ReturnsTourData()
        {
            // Arrange
            var tour = new Tour { Id = 1, Name = "Test Tour" };
            _tourRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync(tour);

            // Act
            var result = await _migrationService.ExportTour(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            var json = System.Text.Encoding.UTF8.GetString(result);
            var deserializedTour = JsonSerializer.Deserialize<Tour>(json);
            Assert.That(deserializedTour.Id, Is.EqualTo(tour.Id));
            Assert.That(deserializedTour.Name, Is.EqualTo(tour.Name));
        }

        [Test]
        public void ExportTour_TourNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            _tourRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((Tour)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _migrationService.ExportTour(1));
            Assert.That(ex.Message, Is.EqualTo("Tour with ID 1 not found."));
        }

        [Test]
        public async Task ImportTour_ValidData_CreatesTour()
        {
            // Arrange
            var tour = new Tour { Id = 1, Name = "Test Tour" };
            var json = JsonSerializer.Serialize(tour);
            var data = System.Text.Encoding.UTF8.GetBytes(json);

            // Act
            await _migrationService.ImportTour(data);

            // Assert
            _tourRepositoryMock.Verify(repo => repo.Create(It.IsAny<Tour>()), Times.Once);
        }

        [Test]
        public async Task ImportTour_InvalidData_DoesNotCreateTour()
        {
            // Arrange
            var data = System.Text.Encoding.UTF8.GetBytes("Invalid JSON");

            // Act & Assert
            Assert.ThrowsAsync<JsonException>(async () => await _migrationService.ImportTour(data));
        }
    }
