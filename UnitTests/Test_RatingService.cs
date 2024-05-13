using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Tourplanner.Entities.TourLog;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace Tourplanner.Services.Tests
{
    [TestFixture]
    public class RatingServiceTests
    {
        [Test]
        public void Calculate_ReturnsAverageRating_WhenTourLogsExist()
        {
            // Arrange
            var tourLogs = new List<TourLog>
            {
                new TourLog { TourLogId = 1, TourId = 1, Rating = 4 },
                new TourLog { TourLogId = 2, TourId = 1, Rating = 5 },
                new TourLog { TourLogId = 3, TourId = 1, Rating = 3 }
            };
            
            var moqTourLogRepository = new Mock<ITourLogRepository>();
            moqTourLogRepository.Setup(repo => repo.GetTourLogsForTour(1)).Returns(tourLogs);

            var ratingService = new RatingService(moqTourLogRepository.Object);

            // Act
            var result = ratingService.Calculate(1);

            // Assert
            Assert.That(4.0f == result); // The expected average rating is 4.0 (average of 4, 5, and 3)
        }

        [Test]
        public void Calculate_ReturnsZero_WhenNoTourLogsExist()
        {
            // Arrange
            var moqTourLogRepository = new Mock<ITourLogRepository>();
            moqTourLogRepository.Setup(repo => repo.GetTourLogsForTour(1)).Returns(new List<TourLog>());

            var ratingService = new RatingService(moqTourLogRepository.Object);

            // Act
            var result = ratingService.Calculate(1);

            // Assert
            Assert.That(0.0f == result); // The expected result is 0.0 since there are no tour logs
        }
    }
}