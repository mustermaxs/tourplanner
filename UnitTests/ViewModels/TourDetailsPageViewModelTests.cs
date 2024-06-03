// using Client.Components;
// using Moq;
// using Client.Dao;
// using Client.Models;
// using Client.ViewModels;
//
// namespace Client.Tests.ViewModels
// {
//     [TestFixture]
//     public class TourDetailsPageViewModelTests
//     {
//         private Mock<ITourDao> _mockTourDao;
//         private Mock<ITourLogDao> _mockTourLogDao;
//         private TourDetailsPageViewModel _viewModel;
//         private Mock<PopupViewModel> _mockPopupViewVM;
//         private Mock<IReportService> _mockReportService;
//
//         [SetUp]
//         public void SetUp()
//         {
//             _mockTourDao = new Mock<ITourDao>();
//             _mockTourLogDao = new Mock<ITourLogDao>();
//             _mockPopupViewVM.Setup(m => m.Open(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PopupStyle>()));
//             _mockReportService = new Mock<IReportService>();
//
//             _viewModel = new TourDetailsPageViewModel(_mockTourDao.Object, _mockTourLogDao.Object, _mockPopupViewVM.Object, _mockReportService.Object);
//         }
//
//         [Test]
//         public async Task DeleteTour_ShouldDeleteTourAndTourLogs()
//         {
//             // Arrange
//             var tour = new Tour { Id = 1 };
//             var tourLogs = new List<TourLog> { new TourLog(), new TourLog() };
//
//             _viewModel.Tour = tour;
//             _viewModel.TourLogs = tourLogs;
//
//             _mockTourDao.Setup(dao => dao.Delete(tour)).Returns(Task.CompletedTask);
//             _mockTourLogDao.Setup(dao => dao.Delete(It.IsAny<TourLog>())).Returns(Task.CompletedTask);
//
//             // Act
//             await _viewModel.DeleteTour();
//
//             // Assert
//             _mockTourDao.Verify(dao => dao.Delete(tour), Times.Once);
//             foreach (var log in tourLogs)
//             {
//                 _mockTourLogDao.Verify(dao => dao.Delete(log), Times.Once);
//             }
//         }
//     }
// }