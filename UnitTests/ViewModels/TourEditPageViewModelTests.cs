// using Moq;
// using Microsoft.AspNetCore.Components;
// using Client.Models;
// using Client.Dao;
// using NUnit.Framework;
// using System;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Client.Components;
// using Client.ViewModels;
//
// namespace Client.Tests.ViewModels
// {
//     [TestFixture]
//     public class TourEditPageViewModelTests
//     {
//         private Mock<ITourDao> _mockTourDao;
//         private Mock<NavigationManager> _mockNavigationManager;
//         private TourEditPageViewModel _viewModel;
//         private Mock<PopupViewModel> _mockPopupViewVM;
//
//         [SetUp]
//         public void SetUp()
//         {
//             _mockTourDao = new Mock<ITourDao>();
//             _mockNavigationManager = new Mock<NavigationManager>();
//             _mockPopupViewVM = new Mock<PopupViewModel>();
//             _mockPopupViewVM.Setup(m => m.Open(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<PopupStyle>()));
//
//             _viewModel = new TourEditPageViewModel(_mockNavigationManager.Object, _mockTourDao.Object, _mockPopupViewVM.Object);
//         }
//
//         [Test]
//         public async Task UpdateTour_CallsUpdateOnTourDao()
//         {
//             // Arrange
//             _mockTourDao.Setup(dao => dao.Update(It.IsAny<Tour>())).Returns(Task.CompletedTask);
//
//             // Act
//             await _viewModel.UpdateTour();
//
//             // Assert
//             _mockTourDao.Verify(dao => dao.Update(It.IsAny<Tour>()), Times.Once);
//         }
//
//     }
// }