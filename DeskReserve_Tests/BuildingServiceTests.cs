using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using DeskReserve.Services;
using Moq;

namespace DeskReserve.Tests.Service
{
    public class BuildingServiceTests
    {
        private BuildingService _buildingService;
        private Mock<IBuildingRepository> _buildingRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _buildingRepositoryMock = new Mock<IBuildingRepository>();
            _buildingService = new BuildingService(_buildingRepositoryMock.Object);
        }

        [Test]
        public async Task NewEntity_ValidCreation_ReturnsTrue()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            _buildingRepositoryMock.Setup(repo => repo.CreateAsync(It.Is<Building>(b =>
                                          b.City == buildingDto.City &&
                                          b.StreetAddress == buildingDto.StreetAddress &&
                                          b.Neighbourhood == buildingDto.Neighbourhood &&
                                          b.FloorsCount == buildingDto.FloorsCount)))
                                          .ReturnsAsync(true);

            var result = await _buildingService.AddNew(buildingDto);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task NewEntity_InvalidCreation_ReturnsFalse()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            var newBuilding = new Building
            {
                BuildingId = new Guid(),
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            _buildingRepositoryMock.Setup(repo => repo.CreateAsync(newBuilding)).ReturnsAsync(false);
            var result = await _buildingService.AddNew(buildingDto);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task Erase_BuildingExistsAndDeletionSucceeds_ReturnsTrue()
        {
            var guid = new Guid();

            var existingBuilding = new Building
            {
                BuildingId = guid,
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync(existingBuilding);
            _buildingRepositoryMock.Setup(repo => repo.DeleteAsync(existingBuilding)).ReturnsAsync(true);

            var result = await _buildingService.DeleteBuilding(existingBuilding.BuildingId);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Erase_BuildingDoesNotExist_ReturnsFalse()
        {
            var guid = new Guid();

            _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync((Building)null);
            var result = await _buildingService.DeleteBuilding(guid);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task Update_BuildingExistsAndUpdateSucceeds_ReturnsTrue()
        {
            var guid = new Guid();

            var existingBuilding = new Building
            {
                BuildingId = guid,
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            var updatedDto = new BuildingDto
            {
                City = "City12",
                StreetAddress = "StreetAddress12",
                Neighbourhood = "Neighbourhood12",
                FloorsCount = 12
            };

            var newBuilding = new Building
            {
                BuildingId = guid,
                City = "City12",
                StreetAddress = "StreetAddress12",
                Neighbourhood = "Neighbourhood12",
                FloorsCount = 12
            };

            _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync(existingBuilding);
            _buildingRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Building>())).ReturnsAsync(true);

            var result = await _buildingService.UpdateBuilding(guid, updatedDto);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task Update_BuildingDoesNotExist_ReturnsFalse()
        {
            var guid = new Guid();
            _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ThrowsAsync(new DataNotFound());

            AsyncTestDelegate result = () => _buildingService.UpdateBuilding(guid, new BuildingDto());
            Assert.ThrowsAsync<DataNotFound>(result);
        }
    }
}
