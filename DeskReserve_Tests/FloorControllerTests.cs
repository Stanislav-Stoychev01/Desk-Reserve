using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using DeskReserve.Controllers;
using DeskReserve.Interfaces;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Http;

namespace Desk_Reserve.Tests
{
    [TestFixture]
    public class FloorControllerTests
    {
        [Test]
        public async Task Get_ReturnsOkWithFloors()
        {
            var mockService = new Mock<IFloorService>();
            var floors = new List<Floor>
            {
                new Floor { FloorId = new Guid() },
                new Floor { FloorId = new Guid() }
            };

            mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(floors);

            var controller = new FloorController(mockService.Object);

            var result = await controller.Get();
            var okObjectResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.IsInstanceOf<IEnumerable<Floor>>(okObjectResult.Value);
        }

        [Test]
        public async Task Get_ReturnsNotFoundWhenNoFloors()
        {
            var mockService = new Mock<IFloorService>();
            mockService.Setup(service => service.GetAllAsync()).ReturnsAsync((IEnumerable<Floor>)null);

            var controller = new FloorController(mockService.Object);

            var result = await controller.Get();
            var resultStatusCode = result.Result as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status404NotFound, resultStatusCode.StatusCode);
        }

        [Test]
        public async Task Get_ReturnsOkObjectResultWhenFloorExists()
        {
            var id = Guid.NewGuid();
            var expectedFloor = new FloorDto { FloorNumber = 1, HasElevator = true, FloorCoveringType = null };

            var mockService = new Mock<IFloorService>();
            mockService.Setup(service => service.GetOneAsync(id))
                .ReturnsAsync(expectedFloor);

            var controller = new FloorController(mockService.Object);

            var result = await controller.Get(id);
            var okObjectResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.IsInstanceOf<FloorDto>(okObjectResult.Value);
        }

        [Test]
        public async Task Get_ReturnsNotFoundResultWhenFloorDoesNotExist()
        {
            var id = Guid.NewGuid();

            var mockService = new Mock<IFloorService>();
            var expectedFloor = (FloorDto)null;

            mockService.Setup(service => service.GetOneAsync(id))
                .ReturnsAsync(expectedFloor);

            var controller = new FloorController(mockService.Object);

            var result = await controller.Get(id);
            var resultStatusCode = result.Result as NotFoundResult;

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status404NotFound, resultStatusCode.StatusCode);
        }

        [Test]
        public async Task Put_ReturnsOkResultOnSuccess()
        {
            var id = Guid.NewGuid();
            var mockFloor = new FloorDto { FloorNumber = 1, HasElevator = true, FloorCoveringType = null };

            var mockService = new Mock<IFloorService>();
            mockService.Setup(service => service.UpdateOneAsync(id, mockFloor))
                .ReturnsAsync(true);

            var controller = new FloorController(mockService.Object);

            var result = await controller.Put(id, mockFloor);
            var resultStatusCode = result as OkResult;

            Assert.IsInstanceOf<OkResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, resultStatusCode.StatusCode);
        }

        [Test]
        public async Task Put_ReturnsInternalServerErrorOnFail()
        {
            var id = Guid.NewGuid();
            var floorDto = new FloorDto();
            var serviceMock = new Mock<IFloorService>();
            serviceMock.Setup(service => service.UpdateOneAsync(id, floorDto)).ReturnsAsync(false);
            var controller = new FloorController(serviceMock.Object);

            var result = await controller.Put(id, floorDto) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsStatusCodeCreatedOnSuccess()
        {
            var floorDto = new FloorDto { FloorNumber = 1, HasElevator = true, FloorCoveringType = "tiles" };
            var serviceMock = new Mock<IFloorService>();
            serviceMock.Setup(service => service.CreateOneAsync(floorDto)).ReturnsAsync(true);
            var controller = new FloorController(serviceMock.Object);

            var result = await controller.Post(floorDto);
            var resultStatusCode = result.Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status201Created, resultStatusCode.StatusCode);
        }

        [Test]
        public async Task Post_ReturnsStatusCodeCreatedOnFail()
        {
            var floorDto = new FloorDto { FloorNumber = 1, HasElevator = true, FloorCoveringType = "tiles" };
            var serviceMock = new Mock<IFloorService>();
            serviceMock.Setup(service => service.CreateOneAsync(floorDto)).ReturnsAsync(false);
            var controller = new FloorController(serviceMock.Object);

            var result = await controller.Post(floorDto);
            var resultStatusCode = result.Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, resultStatusCode.StatusCode);
        }

        [Test]
        public async Task Delete_ReturnsOkResultOnSuccess()
        {
            var id = Guid.NewGuid();
            var serviceMock = new Mock<IFloorService>();
            serviceMock.Setup(service => service.DeleteOneAsync(id)).ReturnsAsync(true);
            var controller = new FloorController(serviceMock.Object);

            var result = await controller.Delete(id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFoundResultOnFail()
        {
            var id = Guid.NewGuid();
            var serviceMock = new Mock<IFloorService>();
            serviceMock.Setup(service => service.DeleteOneAsync(id)).ReturnsAsync(false);
            var controller = new FloorController(serviceMock.Object);

            var result = await controller.Delete(id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
