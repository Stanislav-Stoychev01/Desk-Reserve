using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using DeskReserve.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DeskReserve_Tests.Services_Tests
{
    [TestFixture]
    public class FloorServiceTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllFloors()
        {
            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Floor>());

            var service = new FloorService(repositoryMock.Object);

            var floors = await service.GetAllAsync();

            Assert.IsNotNull(floors);
            Assert.AreEqual(0, floors.Count());
        }

        [Test]
        public async Task GetOneAsync_ValidId_ReturnsFloorDto()
        {
            Guid id = Guid.NewGuid();
            Floor floorEntity = new Floor { FloorId = id };

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(floorEntity);

            var floorService = new FloorService(repositoryMock.Object);

            var result = await floorService.GetOneAsync(id);

            Assert.IsInstanceOf<FloorDto>(result);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetOneAsync_InvalidId_ReturnsNull()
        {
            Guid id = Guid.NewGuid();
            Floor expectedFloor = null;

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(expectedFloor);

            var floorService = new FloorService(repositoryMock.Object);

            var result = await floorService.GetOneAsync(id);

            Assert.IsNull(result);
        }

        [Test]
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

            var floorService = new FloorService(repositoryMock.Object);

            var result = await floorService.UpdateOneAsync(id, floorDto);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteOneAsync_WhenEntityFound_DeletesAndReturnsTrue()
        {
            Guid id = Guid.NewGuid();
            Floor floor = new Floor()
            {
                FloorId = id,
            };

            var repositoryMock = new Mock<IFloorRepository>();
            repositoryMock.Setup(repo => repo.Delete(floor)).ReturnsAsync(true);
            repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(floor);

            var floorService = new FloorService(repositoryMock.Object);

            var result = await floorService.DeleteOneAsync(id);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteOneAsync_WhenEntityNotFound_ThrowsEntityNotFoundException()
        {
            {
                Guid id = Guid.NewGuid();
                Floor floor = null;

                var repositoryMock = new Mock<IFloorRepository>();
                repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(floor);

                var floorService = new FloorService(repositoryMock.Object);

                Assert.ThrowsAsync<EntityNotFoundException>(async () => await floorService.DeleteOneAsync(id));
            }
        }
    }
}
