using DeskReserve.Domain;
using Moq;
using DeskReserve.Data.DBContext.Entity;
using Microsoft.AspNetCore.Http;
using DeskReserve.Exceptions;
using DeskReserve.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using DeskReserve.Interfaces;

namespace DeskReserve_Tests.Controllers_Tests
{
    [TestFixture]
    internal class RoomControllerTest
    {
        private RoomController _roomController;
        private Mock<IRoomService> _roomServiceMock;

        [SetUp]
        public void SetUp()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _roomController = new RoomController(_roomServiceMock.Object);
        }
        [Test]
        public async Task Get_ReturnsOkWithRooms()
        {
            var rooms = new List<Room>
            {
                new Room { RoomId = new Guid()},
                new Room { RoomId = new Guid()}
            };

            _roomServiceMock.Setup(service => service.GetAll()).ReturnsAsync(rooms);

            var result = await _roomController.GetAll();
            var okObjectResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.IsInstanceOf<IEnumerable<Room>>(okObjectResult.Value);

        }

        [Test]
        public async Task Get_ReturnsNotFoundWhenNoRoom()
        {
            _roomServiceMock.Setup(service => service.GetAll()).ReturnsAsync(new List<Room>());

            var result = await _roomController.GetAll();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetById_ExsistingRoomId_ReturnsRoomOkStatus()
        {
            var id = Guid.NewGuid();
            var room = new RoomDto { RoomNumber = 1, HasAirConditioner = false };

            _roomServiceMock.Setup(service => service.Get(id)).ReturnsAsync(room);

            var result = await _roomController.GetById(id);
            var okObjectResult = result.Result as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.IsInstanceOf<RoomDto>(okObjectResult.Value);
        }

        [Test]
        public async Task Get_ReturnsNotFoundResultWhenRoomDoesNotExist()
        {
            var id = Guid.NewGuid();
            _roomServiceMock.Setup(service => service.Get(id)).ThrowsAsync(new EntityNotFoundException());

            var result = await _roomController.GetById(id);
            var resultStatusCode = result.Result as NotFoundObjectResult;

            Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task Put_ReturnsOkResultOnSuccess()
        {
            var id = Guid.NewGuid();
            var roomMock = new RoomDto { RoomNumber = 1, HasAirConditioner = false };

            _roomServiceMock.Setup(service => service.Update(id, roomMock)).ReturnsAsync(true);

            var result = await _roomController.UpdateOne(id, roomMock) as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task Put_ReturnsInternalServerErrorOnFail()
        {
            var id = Guid.NewGuid();
            var roomDto = (RoomDto)null;

            _roomServiceMock.Setup(service => service.Update(id, roomDto)).ThrowsAsync(new EntityNotFoundException());

            var result = await _roomController.UpdateOne(id, roomDto) as NotFoundObjectResult;

            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task Post_ReturnsStatusCodeCreatedOnFail()
        {
            var roomDto = new RoomDto { RoomNumber = 1, HasAirConditioner = false };

            _roomServiceMock.Setup(service => service.Create(roomDto)).ReturnsAsync(false);

            var result = await _roomController.CreateAsync(roomDto);
            var resultStatusCode = result.Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        }


        [Test]
        public async Task Delete_ReturnsNoContentResultOnSuccess()
        {
            var id = Guid.NewGuid();

            _roomServiceMock.Setup(service => service.Delete(id)).ReturnsAsync(true);

            var result = await _roomController.Delete(id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFoundResultOnFail()
        {
            var id = Guid.NewGuid();

            _roomServiceMock.Setup(service => service.Delete(id)).ThrowsAsync(new EntityNotFoundException());

            var result = await _roomController.Delete(id) as NotFoundObjectResult;

            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}
