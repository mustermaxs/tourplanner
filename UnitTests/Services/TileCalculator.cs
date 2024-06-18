using NUnit.Framework;
using Tourplanner.Services;
using Tourplanner.Entities;
using System.Collections.Generic;

namespace Tourplanner.Tests
{
    [TestFixture]
    public class TileCalculatorTests
    {
        private ITileCalculator _tileCalculator;

        [SetUp]
        public void SetUp()
        {
            _tileCalculator = new TileCalculator();
        }

        [Test]
        public void LatLonToTileNumbers_ValidCoordinates_ReturnsExpectedTileNumbers()
        {
            // Arrange
            double lat = 12.995288;
            double lon = 47.82287;
            int zoom = 7;

            // Act
            var result = _tileCalculator.LatLonToTileNumbers(lat, lon, zoom);

            // Assert
            Assert.That(result.xtile, Is.EqualTo(81));
            Assert.That(result.ytile, Is.EqualTo(59));
        }

        [Test]
        public void LatLonToTileNumbers_ZeroCoordinates_ReturnsCenterTileNumbers()
        {
            // Arrange
            double lat = 0;
            double lon = 0;
            int zoom = 10;

            // Act
            var result = _tileCalculator.LatLonToTileNumbers(lat, lon, zoom);

            // Assert
            Assert.That(result.xtile, Is.EqualTo(512));
            Assert.That(result.ytile, Is.EqualTo(512));
        }

        [Test]
        public void GetTileConfigs_ValidBbox_ReturnsExpectedTileConfigs()
        {
            // Arrange
            int zoom = 10;
            Bbox bbox = new Bbox(16.412244, 48.273432, 16.5257, 48.352153);

            // Act
            List<TileConfig> result = _tileCalculator.GetTileConfigs(zoom, bbox);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.Any(r => r.Zoom == 10 && r.X == 558 && r.Y == 354));
            Assert.That(result.Any(r => r.Zoom == 10 && r.X == 559 && r.Y == 354));
        }

        [Test]
        public void GetTileConfigs_SingleTileBbox_ReturnsSingleTileConfig()
        {
            // Arrange
            int zoom = 1;
            Bbox bbox = new Bbox(16.412244, 48.273432, 16.5257, 48.352153);

            // Act
            List<TileConfig> result = _tileCalculator.GetTileConfigs(zoom, bbox);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.Any(r => r.Zoom == 1 && r.X == 1 && r.Y == 0));
        }
    }
}
