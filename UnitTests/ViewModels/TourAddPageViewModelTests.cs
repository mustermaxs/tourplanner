using Moq;
using Microsoft.AspNetCore.Components;
using Client.Models;
using Client.Dao;
using Client.ViewModels;

namespace Client.Tests.ViewModels
{
    [TestFixture]
    public class TourAddPageViewModelTests
    {
        private Mock<ITourDao> _mockTourDao;
        private Mock<NavigationManager> _mockNavigationManager;
        private TourAddPageViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _mockTourDao = new Mock<ITourDao>();
            _mockNavigationManager = new Mock<NavigationManager>();
            _viewModel = new TourAddPageViewModel(_mockNavigationManager.Object, _mockTourDao.Object);
        }

        [Test]
        public async Task AddTour_CallsCreateOnTourDao()
        {
            // Arrange
            _mockTourDao.Setup(dao => dao.Create(It.IsAny<Tour>())).Returns(Task.CompletedTask);

            // Act
            await _viewModel.AddTour();

            // Assert
            _mockTourDao.Verify(dao => dao.Create(It.IsAny<Tour>()), Times.Once);
        }
        
    }
}