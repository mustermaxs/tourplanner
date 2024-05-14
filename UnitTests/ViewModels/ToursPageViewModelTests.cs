using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models;
using Client.Dao;
using Moq;
using NUnit.Framework;

namespace Client.Tests.ViewModels
{
    [TestFixture]
    public class ToursPageViewModelTests
    {
        private Mock<ITourDao> _mockTourDao;
        private ToursPageViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockTourDao = new Mock<ITourDao>();
            _viewModel = new ToursPageViewModel(_mockTourDao.Object);
        }

        [Test]
        public async Task GetToursAsync_SuccessfullyFetchesTours()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { Name = "Tour 1", Id = 1 },
                new Tour { Name = "Tour 2", Id = 2 }
            };
            _mockTourDao.Setup(dao => dao.ReadMultiple()).ReturnsAsync(tours);

            // Act
            await _viewModel.GetToursAsync();

            // Assert
            Assert.That(tours == _viewModel.Tours);
        }
    }
}
