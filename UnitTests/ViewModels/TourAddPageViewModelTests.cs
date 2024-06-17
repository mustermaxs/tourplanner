// using Client.Components;
// using Moq;
// using Microsoft.AspNetCore.Components;
// using Client.Models;
// using Client.Dao;
// using Client.Services;
// using Client.ViewModels;
//
// namespace Client.Tests.ViewModels
// {
//     [TestFixture]
//     public class TourAddPageViewModelTests
//     {
//         private Mock<ITourDao> _mockTourDao;
//         private Mock<NavigationManager> _mockNavigationManager;
//         private TourAddPageViewModel _viewModel;
//         private Mock<PopupViewModel> _mockPopupViewVM;
//         private Mock<IHttpService> _mockHttpService;
//         private Mock<IGeoService> _mockGeoService;
//
//         [SetUp]
//         public void SetUp()
//         {
//             _mockTourDao = new Mock<ITourDao>();
//             _mockNavigationManager = new Mock<NavigationManager>();
//             _mockPopupViewVM = new Mock<PopupViewModel>();
//             _mockPopupViewVM.Setup(m => m.Open(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PopupStyle>()));
//             _mockHttpService = new Mock<IHttpService>();
//             _mockGeoService = new Mock<IGeoService>();
//             _viewModel = new TourAddPageViewModel(_mockNavigationManager.Object, _mockTourDao.Object, _mockPopupViewVM.Object, _mockHttpService.Object, _mockGeoService.Object);
//         }
//
//         [Test]
//         public async Task AddTour_CallsCreateOnTourDao()
//         {
//             // Arrange
//             _mockTourDao.Setup(dao => dao.Create(It.IsAny<Tour>())).Returns(Task.CompletedTask);
//
//             // Act
//             await _viewModel.AddTour();
//
//             // Assert
//             _mockTourDao.Verify(dao => dao.Create(It.IsAny<Tour>()), Times.Once);
//         }
//         
//     }
// }