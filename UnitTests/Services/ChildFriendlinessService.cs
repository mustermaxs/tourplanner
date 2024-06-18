using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tourplanner.Entities.Tours;
using Tourplanner.Entities.TourLogs;
using Tourplanner.Repositories;
using Tourplanner.Services;
using Tourplanner.Exceptions;

namespace UnitTests.Services;

[TestFixture]
    public class ChildFriendlinessServiceTests
    {
        private Mock<ITourRepository> _tourRepositoryMock;
        private Mock<ITourLogRepository> _tourLogRepositoryMock;
        private ChildFriendlinessService _childFriendlinessService;

        [SetUp]
        public void SetUp()
        {
            _tourRepositoryMock = new Mock<ITourRepository>();
            _tourLogRepositoryMock = new Mock<ITourLogRepository>();
            _childFriendlinessService = new ChildFriendlinessService(_tourRepositoryMock.Object, _tourLogRepositoryMock.Object);
        }

        [Test]
        public void GetAverageDifficulty_NoLogs_ReturnsZero()
        {
            // Arrange
            var logs = new List<TourLog>();

            // Act
            var result = _childFriendlinessService.GetAverageDifficulty(logs);

            // Assert
            Assert.That(result, Is.EqualTo(0.0f));
        }

        [Test]
        public void GetAverageDifficulty_WithLogs_ReturnsAverage()
        {
            // Arrange
            var logs = new List<TourLog>
            {
                new TourLog { Difficulty = 2 },
                new TourLog { Difficulty = 4 },
                new TourLog { Difficulty = 6 }
            };

            // Act
            var result = _childFriendlinessService.GetAverageDifficulty(logs);

            // Assert
            Assert.That(result, Is.EqualTo(4.0f));
        }

        [Test]
        public void GetMaxTourDistance_WithTours_ReturnsMaxDistance()
        {
            // Arrange
            var tours = new List<Tour>
            {
                new Tour { Distance = 5 },
                new Tour { Distance = 10 },
                new Tour { Distance = 15 }
            };

            // Act
            var result = _childFriendlinessService.GetMaxTourDistance(tours);

            // Assert
            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public async Task Calculate_TourNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            _tourRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Tour>());

            // Act & Assert
            var ex = Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _childFriendlinessService.Calculate(1));
            Assert.That(ex.Message, Is.EqualTo("Tour not found"));
        }

    }
