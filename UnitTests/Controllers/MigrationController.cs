using System.IO;
using System.Text;
using System.Threading.Tasks;
using Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Tourplanner.Exceptions;
using Tourplanner.Services;

namespace Tourplanner.UnitTests.Controllers
{
    [TestFixture]
    public class MigrationControllerTests
    {
        private MigrationController _controller;
        private Mock<IMigrationService> _mockService;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IMigrationService>();
            _controller = new MigrationController(_mockService.Object);
        }

        [Test]
        public async Task ExportTour_TourExists_ReturnsFileResult()
        {
            // Arrange
            int tourId = 1;
            var jsonData = new byte[] { 1, 2, 3 };
            _mockService.Setup(s => s.ExportTour(tourId)).ReturnsAsync(jsonData);

            // Act
            var result = await _controller.ExportTour(tourId) as FileContentResult;

            // Assert
            Assert.That(result, Is.InstanceOf<FileContentResult>());
            Assert.That(result.ContentType, Is.EqualTo("application/json"));
            Assert.That(result.FileContents, Is.EqualTo(jsonData));
        }

        [Test]
        public async Task ExportTour_TourNotFound_ReturnsNotFound()
        {
            // Arrange
            int tourId = 1;
            _mockService.Setup(s => s.ExportTour(tourId)).ThrowsAsync(new ResourceNotFoundException("Tour not found"));

            // Act
            var result = await _controller.ExportTour(tourId);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task ImportTour_ValidFile_ReturnsOk()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = new MemoryStream(Encoding.UTF8.GetBytes("file content"));
            mockFile.Setup(_ => _.OpenReadStream()).Returns(content);
            mockFile.Setup(_ => _.FileName).Returns("test.json");
            mockFile.Setup(_ => _.Length).Returns(content.Length);

            // Act
            var result = await _controller.ImportTour(mockFile.Object);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task ImportTour_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            IFormFile file = null;

            // Act
            var result = await _controller.ImportTour(file);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}
