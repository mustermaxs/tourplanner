using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TourLogDao.Models;
using TourLogDao.Repositories;

namespace Tests
{
    [TestClass]
    public class TourLogDaoTests
    {
        [TestMethod]
        public void GetTourLog_ValidTourId_ReturnsExpectedTourLog()
        {
            // Arrange
            var mockRepository = new Mock<ITourLogRepository>();
            mockRepository.Setup(r => r.GetTourLog(It.IsAny<int>()))
                .Returns(new TourLog { TourId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2) });

            var dao = new TourLogDao(mockRepository.Object);
