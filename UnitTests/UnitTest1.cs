using Moq;
using Tourplanner.Entities.Tour;
using Tourplanner.Entities.TourLog;
using Tourplanner.Exceptions;
using Tourplanner.Repositories;
using Tourplanner.Services;

namespace UnitTests;


[TestFixture]
public class ChildFriendlinessServiceTests
{
    [Test]
    public async Task Calculate_ReturnsNormalizedSum()
    {
        // Arrange
        var tour = new Tour { TourId = 1, Distance = 50, EstimatedTime = 120 };
        var tourLogs = new List<TourLog>
        {
            new TourLog { TourLogId = 1, TourId = 1, Difficulty = 3 },
            new TourLog { TourLogId = 2, TourId = 1, Difficulty = 4 }
        };
            
        var moqTourRepo = new Mock<ITourRepository>();
        moqTourRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Tour> { tour });

        var moqLogRepo = new Mock<ITourLogRepository>();
        moqLogRepo.Setup(repo => repo.GetTourLogsForTour(1)).Returns(tourLogs);

        var service = new ChildFriendlinessService(moqTourRepo.Object, moqLogRepo.Object);

        // Act
        var result = await service.Calculate(1);

        // Assert
        Console.WriteLine(result);
        Assert.That(10.0f == result); // Adjust the expected result based on your calculations
    }

    [Test]
    public void Calculate_ThrowsException_WhenTourNotFound()
    {
        // Arrange
        var moqTourRepo = new Mock<ITourRepository>();
        moqTourRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Tour>());

        var moqLogRepo = new Mock<ITourLogRepository>();

        var service = new ChildFriendlinessService(moqTourRepo.Object, moqLogRepo.Object);

        // Act & Assert
        Assert.ThrowsAsync<ResourceNotFoundException>(async () => await service.Calculate(1));
    }
}