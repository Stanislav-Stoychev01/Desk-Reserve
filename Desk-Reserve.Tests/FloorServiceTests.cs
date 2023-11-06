using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Interfaces;
using DeskReserve.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Desk_Reserve.Tests
{
    [TestClass]
    public class FloorServiceTests
    {
        [TestMethod]
        public async Task GetAllAsync_ReturnsAllFloors()
        {
            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Floor>());

            var mapperMock = new Mock<IMapper>();
            var service = new FloorService(repositoryMock.Object, mapperMock.Object);

            var floors = await service.GetAllAsync();

            Assert.IsNotNull(floors);
            Assert.AreEqual(0, floors.Count());
        }

        [TestMethod]
        public async Task GetOneAsync_ValidId_ReturnsFloorDto()
        {
            Guid id = Guid.NewGuid();
            Floor floorEntity = new Floor { FloorId = id };

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(floorEntity);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.MapProperties(floorEntity, It.IsAny<FloorDto>())).Returns<Floor, FloorDto>((entity, dto) =>
            {
                dto.FloorNumber = entity.FloorNumber;
                dto.HasElevator = entity.HasElevator;
                dto.FloorCoveringType = entity.FloorCoveringType;
                return dto;
            });
            var floorService = new FloorService(repositoryMock.Object, mapperMock.Object);

            var result = await floorService.GetOneAsync(id);

            Assert.IsInstanceOfType<FloorDto>(result);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetOneAsync_InvalidId_ReturnsNull()
        {
            Guid id = Guid.NewGuid();
            Floor expectedFloor = null;

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(expectedFloor);

            var mapperMock = new Mock<IMapper>();
            var floorService = new FloorService(repositoryMock.Object, mapperMock.Object);

            var result = await floorService.GetOneAsync(id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateOneAsync_ValidIdAndDto_ReturnsTrue()
        {
            Guid id = Guid.NewGuid();
            FloorDto floorDto = new FloorDto
            {
                FloorNumber = 2,
                HasElevator = true,
                FloorCoveringType = "tiles"
            };

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.Update(It.IsAny<Floor>())).ReturnsAsync(true);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.MapProperties(floorDto, It.IsAny<Floor>())).Returns<FloorDto, Floor>((dto, entity) =>
            {
                entity.FloorNumber = dto.FloorNumber;
                entity.HasElevator = dto.HasElevator;
                entity.FloorCoveringType = dto.FloorCoveringType;
                return entity;
            });

            var floorService = new FloorService(repositoryMock.Object, mapperMock.Object);

            var result = await floorService.UpdateOneAsync(id, floorDto);

            Assert.IsTrue(result); 
        }

        [TestMethod]
        public async Task DeleteOneAsync_ValidId_ReturnsTrue()
        {
            Guid id = Guid.NewGuid();

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(true);

            var mapperMock = new Mock<IMapper>();
            var floorService = new FloorService(repositoryMock.Object, mapperMock.Object);

            var result = await floorService.DeleteOneAsync(id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteOneAsync_InvalidId_ReturnsFalse()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.Delete(id)).ReturnsAsync(false);

            var mapperMock = new Mock<IMapper>();
            var floorService = new FloorService(repositoryMock.Object, mapperMock.Object);

            var result = await floorService.DeleteOneAsync(id);

            Assert.IsFalse(result);
        }
    }
}
