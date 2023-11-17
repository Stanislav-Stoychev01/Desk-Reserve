using DeskReserve.Controllers;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeskReserve_Tests.Controllers_Tests
{
    [TestFixture]
    public class BuildingControllerTests
    {
        private BuildingController _buildingController;
        private Mock<IBuildingService> _buildingServiceMock;

        [SetUp]
        public void Setup()
        {
            _buildingServiceMock = new Mock<IBuildingService>();
            _buildingController = new BuildingController(_buildingServiceMock.Object);
        }

        [Test]
        public async Task Get_Called_ReturnsListOfBuildingsWithOkStatus()
        {
            var buildings = new List<Building>
            {
                new Building { BuildingId = Guid.NewGuid(),
                               City = "City1",
                               StreetAddress = "Street 1",
                               Neighbourhood = "Neighbourhood 1",
                               FloorsCount = 1 },
                new Building { BuildingId = Guid.NewGuid(),
                               City = "City2",
                               StreetAddress = "Street2",
                               Neighbourhood = "Neighbourhood2",
                               FloorsCount = 2 }
            };

            _buildingServiceMock.Setup(service => service.GetAll()).ReturnsAsync(buildings);

            var result = await _buildingController.Get();
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task Get_NoBuildingsFound_ReturnsEmptyList()
        {
            _buildingServiceMock.Setup(service => service.GetAll()).ReturnsAsync(new List<Building>());

            var result = await _buildingController.Get();
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetById_ExistingBuildingId_ReturnsBuildingWithOkStatus()
        {
            var id = new Guid();

            var building = new Building
            {
                BuildingId = id,
                City = "City1",
                StreetAddress = "Street 1",
                Neighbourhood = "Neighbourhood 1",
                FloorsCount = 1
            };

            _buildingServiceMock.Setup(service => service.GetBuildingById(id)).ReturnsAsync(building);

            var result = await _buildingController.GetById(id);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetById_NonExistingBuildingId_ReturnsNotFoundStatus()
        {
            var id = new Guid();
            _buildingServiceMock.Setup(service => service.GetBuildingById(id)).ThrowsAsync(new DataNotFound());

            var result = await _buildingController.GetById(id);
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task Post_ValidBuilding_ReturnsCreatedStatus()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "Street1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            _buildingServiceMock.Setup(service => service.AddNew(buildingDto)).ReturnsAsync(true);

            var result = await _buildingController.Post(buildingDto);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Post_InvalidCreation_ReturnsInternalServerErrorStatus()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "Street1",
                Neighbourhood = "Neighbourhood1",
                FloorsCount = 1
            };

            _buildingServiceMock.Setup(service => service.AddNew(buildingDto)).ReturnsAsync(false);

            var result = await _buildingController.Post(buildingDto);
            Assert.IsInstanceOf<StatusCodeResult>(result);
        }

        [Test]
        public async Task Delete_ExistingBuildingId_ReturnsOkStatus()
        {
            var guid = new Guid();
            _buildingServiceMock.Setup(service => service.DeleteBuilding(guid)).ReturnsAsync(true);

            var result = await _buildingController.Delete(guid);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Delete_NonExistingBuildingId_ReturnsNotFoundStatus()
        {
            var guid = new Guid();
            _buildingServiceMock.Setup(service => service.DeleteBuilding(guid)).ThrowsAsync(new DataNotFound());

            var result = await _buildingController.Delete(guid);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Put_UpdatesBuilding_ReturnsOkStatus()
        {
            var id = new Guid();

            var buildingDto = new BuildingDto
            {
                City = "City12",
                StreetAddress = "Street 12",
                Neighbourhood = "Neighbourhood 12",
                FloorsCount = 12
            };

            _buildingServiceMock.Setup(service => service.UpdateBuilding(id, buildingDto)).ReturnsAsync(true);

            var result = await _buildingController.Put(id, buildingDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Put_InvalidUpdate_ReturnsNotFoundOrErrorStatus()
        {
            var id = new Guid();

            var buildingDto = new BuildingDto
            {
                City = "City12",
                StreetAddress = "Street 12",
                Neighbourhood = "Neighbourhood 12",
                FloorsCount = 12
            };

            _buildingServiceMock.Setup(service => service.UpdateBuilding(id, buildingDto)).ThrowsAsync(new DataNotFound());

            var result = await _buildingController.Put(id, buildingDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}