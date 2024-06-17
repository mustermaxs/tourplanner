// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Client.Components;
// using Client.Models;
// using Client.Dao;
// using Client.ViewModels;
// using Moq;
// using NUnit.Framework;
//
// namespace Client.Tests.ViewModels
// {
//     [TestFixture]
//     public class ToursPageViewModelTests
//     {
//         private Mock<ITourDao> _mockTourDao;
//         private ToursPageViewModel _viewModel;
//         private Mock<IReportService> _mockReportService;
//         private Mock<PopupViewModel> _mockPopupViewVM;
//
//         [SetUp]
//         public void SetUp()
//         {
//             _mockTourDao = new Mock<ITourDao>();
//             _mockReportService = new Mock<IReportService>();
//             _mockPopupViewVM = new Mock<PopupViewModel>();
//
//             _mockPopupViewVM.Setup(m => m.Open(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PopupStyle>()));
//             // ToursPageViewModel(
//             //     ITourDao tourDao, 
//             //     IReportService reportService, 
//             //     PopupViewModel popupViewModel)
//             _viewModel = new ToursPageViewModel(_mockTourDao.Object, _mockReportService.Object, _mockPopupViewVM.Object);
//         }
//
//         [Test]
//         public async Task GetToursAsync_SuccessfullyFetchesTours()
//         {
//             // Arrange
//             var tours = new List<Tour>
//             {
//                 new Tour { Name = "Tour 1", Id = 1 },
//                 new Tour { Name = "Tour 2", Id = 2 }
//             };
//             _mockTourDao.Setup(dao => dao.ReadMultiple()).ReturnsAsync(tours);
//
//             // Act
//             await _viewModel.GetToursAsync();
//
//             // Assert
//             Assert.That(tours == _viewModel.Tours);
//         }
//     }
// }
