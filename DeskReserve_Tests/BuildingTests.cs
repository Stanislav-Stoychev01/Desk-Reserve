using DeskReserve.Controllers;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeskReserve.Tests.Controllers
{
    [TestFixture]
    public class BuildingControllerTests
    {
        private BuildingController _buildingController;
        private BuildingService _buildingService;
        private Mock<IBuildingService> _buildingServiceMock;
        private Mock<IRepository> _buildingRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _buildingServiceMock = new Mock<IBuildingService>();
            _buildingRepositoryMock = new Mock<IRepository>();
            _buildingController = new BuildingController(_buildingServiceMock.Object);
            _buildingService = new BuildingService(_buildingRepositoryMock.Object);
        }

        [Test]
        public async Task GetReturnsListOfBuildings()
        {
            var buildings = new List<Building>
            {
                new Building { BuildingId = Guid.NewGuid(),
                               City = "City1",
                               StreetAddress = "Street 1",
                               Neighbourhood = "Neighbourhood 1",
                               Floors = 1 },
                new Building { BuildingId = Guid.NewGuid(),
                               City = "City2",
                               StreetAddress = "Street2",
                               Neighbourhood = "Neighbourhood2",
                               Floors = 2 }
            };

            _buildingServiceMock.Setup(service => service.GetAll()).ReturnsAsync(buildings);

            var result = await _buildingController.Get();
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            var okResult = (ObjectResult)result.Result;
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedBuildings = (List<Building>)okResult.Value;
            CollectionAssert.AreEqual(buildings, returnedBuildings);
        }

        [Test]
        public async Task GetReturnsNotFound()
        {
            _buildingServiceMock.Setup(service => service.GetAll()).ReturnsAsync(new List<Building>());

            var result = await _buildingController.Get();
            Assert.IsInstanceOf<NotFoundResult>(result.Result);

            var errorResult = (NotFoundResult)result.Result;
            Assert.AreEqual(404, errorResult.StatusCode);
        }

        [Test]
        public async Task GetIdReturnsBuilding()
        {
            var building = new Building
            {
                BuildingId = new Guid("18e47476-06bb-4a7c-a348-aaa4e0d0e7c0"),
                City = "City1",
                StreetAddress = "Street 1",
                Neighbourhood = "Neighbourhood 1",
                Floors = 1
            };

            _buildingServiceMock.Setup(service => service.GetOne(new Guid("18e47476-06bb-4a7c-a348-aaa4e0d0e7c0"))).ReturnsAsync(building);

            var result = await _buildingController.GetById(new Guid("18e47476-06bb-4a7c-a348-aaa4e0d0e7c0"));
            Assert.IsInstanceOf<ObjectResult>(result.Result);

            var okResult = (ObjectResult)result.Result;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(building, okResult.Value);
        }

        [Test]
        public async Task GetByIdReturnsNotFound()
        {
            _buildingServiceMock.Setup(service => service.GetOne(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"))).ReturnsAsync((Building)null);

            var result = await _buildingController.GetById(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result.Result);

            var errorResult = (NotFoundResult)result.Result;
            Assert.AreEqual(404, errorResult.StatusCode);
        }

        [Test]
        public async Task PostReturnsCreated()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "Street1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            _buildingServiceMock.Setup(service => service.NewEntity(buildingDto)).ReturnsAsync(true);

            var result = await _buildingController.Post(buildingDto);
            Assert.IsInstanceOf<StatusCodeResult>(result.Result);

            var createdResult = (StatusCodeResult)result.Result;
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task PostReturnsInternalServerError()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "Street1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            _buildingServiceMock.Setup(service => service.NewEntity(buildingDto)).ReturnsAsync(false);

            var result = await _buildingController.Post(buildingDto);
            Assert.IsInstanceOf<StatusCodeResult>(result.Result);

            var errorResult = (StatusCodeResult)result.Result;
            Assert.AreEqual(500, errorResult.StatusCode);
        }

        [Test]
        public async Task DeleteReturnsOk()
        {
            var guid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            _buildingServiceMock.Setup(service => service.Erase(guid)).ReturnsAsync(true);

            var result = await _buildingController.Delete(guid);
            Assert.IsInstanceOf<OkResult>(result);

            var okResult = (OkResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task DeleteReturnsNotFound()
        {
            var guid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
            _buildingServiceMock.Setup(service => service.Erase(guid)).ReturnsAsync(false);

            var result = await _buildingController.Delete(Guid.NewGuid());
            Assert.IsInstanceOf<NotFoundResult>(result);

            var errorResult = (NotFoundResult)result;
            Assert.AreEqual(404, errorResult.StatusCode);
        }

        [Test]
        public async Task PutReturnsOk()
        {
            var id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");

            var buildingDto = new BuildingDto
            {
                City = "City12",
                StreetAddress = "Street 12",
                Neighbourhood = "Neighbourhood 12",
                Floors = 12
            };

            _buildingServiceMock.Setup(service => service.Update(id, buildingDto)).ReturnsAsync(true);

            var result = await _buildingController.Put(id, buildingDto);
            Assert.IsInstanceOf<OkResult>(result);

            var okResult = (OkResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task PutReturnsNotFound()
        {
            var id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7");

            var buildingDto = new BuildingDto
            {
                City = "City12",
                StreetAddress = "Street 12",
                Neighbourhood = "Neighbourhood 12",
                Floors = 12
            };

            _buildingServiceMock.Setup(service => service.Update(id, buildingDto)).ReturnsAsync(false);

            var result = await _buildingController.Put(id, buildingDto);
            Assert.IsInstanceOf<StatusCodeResult>(result);

            var errorResult = (StatusCodeResult)result;
            Assert.AreEqual(500, errorResult.StatusCode);
        }

        [Test]
        public void UpgradeDto()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            var result = _buildingService.UpgradeDto(buildingDto);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.BuildingId);
            Assert.AreEqual(buildingDto.City, result.City);
            Assert.AreEqual(buildingDto.StreetAddress, result.StreetAddress);
            Assert.AreEqual(buildingDto.Neighbourhood, result.Neighbourhood);
            Assert.AreEqual(buildingDto.Floors, result.Floors);
        }

        [Test]
        public void UpgradeDtoWithId()
        {
            var id = Guid.NewGuid();

            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            var result = _buildingService.UpgradeDto(id, buildingDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.BuildingId);
            Assert.AreEqual(buildingDto.City, result.City);
            Assert.AreEqual(buildingDto.StreetAddress, result.StreetAddress);
            Assert.AreEqual(buildingDto.Neighbourhood, result.Neighbourhood);
            Assert.AreEqual(buildingDto.Floors, result.Floors);
        }

        [Test]
        public async Task NewEntityReturnsTrue()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            _buildingRepositoryMock.Setup(repo => repo.CreateAsync(It.Is<Building>(b =>
                                          b.City == buildingDto.City &&
                                          b.StreetAddress == buildingDto.StreetAddress &&
                                          b.Neighbourhood == buildingDto.Neighbourhood &&
                                          b.Floors == buildingDto.Floors)))
                                          .ReturnsAsync(true);

            var result = await _buildingService.NewEntity(buildingDto);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task NewEntityReturnsFalse()
        {
            var buildingDto = new BuildingDto
            {
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            var newBuilding = new Building
            {
                BuildingId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"),
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            _buildingRepositoryMock.Setup(repo => repo.CreateAsync(newBuilding)).ReturnsAsync(false);
            var result = await _buildingService.NewEntity(buildingDto);

            Assert.IsFalse(result);
        }

        [Test]
         public async Task EraseReturnsTrue()
         {
             var guid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7");

             var existingBuilding = new Building
             {
                 BuildingId = guid,
                 City = "City1",
                 StreetAddress = "StreetAddress1",
                 Neighbourhood = "Neighbourhood1",
                 Floors = 1
             };

             _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync(existingBuilding);
             _buildingRepositoryMock.Setup(repo => repo.DeleteAsync(existingBuilding)).ReturnsAsync(true);

             var result = await _buildingService.Erase(existingBuilding.BuildingId);
             Assert.IsTrue(result);
         }

         [Test]
         public async Task EraseReturnsFalse()
         {
             var guid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7");

             _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync((Building)null);
             var result = await _buildingService.Erase(guid);

             Assert.IsFalse(result);
         }

        [Test]
        public async Task UpdateReturnsTrue2()
        {
            var guid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7");

            var existingBuilding = new Building
            {
                BuildingId = guid,
                City = "City1",
                StreetAddress = "StreetAddress1",
                Neighbourhood = "Neighbourhood1",
                Floors = 1
            };

            var updatedDto = new BuildingDto
            {
                City = "City12",
                StreetAddress = "StreetAddress12",
                Neighbourhood = "Neighbourhood12",
                Floors = 12
            };

            _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync(existingBuilding);
            _buildingRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Building>())).ReturnsAsync(true);

            var result = await _buildingService.Update(guid, updatedDto);

            Assert.IsTrue(result);
        }
        
        [Test]
         public async Task UpdateReturnsFalse()
         {
             _buildingRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Building)null);
             var result = await _buildingService.Update(Guid.NewGuid(), new BuildingDto());

             Assert.IsFalse(result);
         }
    }
}