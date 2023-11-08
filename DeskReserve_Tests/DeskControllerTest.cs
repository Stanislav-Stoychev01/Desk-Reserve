using Microsoft.AspNetCore.Mvc;
using Moq;
using DeskReserve.Controllers;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Http;
using DeskReserve.Exception;


namespace DeskReserve_Tests
{
	[TestFixture]
	public class DeskControllerTest
	{
		[Test]
		public async Task Get_ReturnsOkWithDesks()
		{
			var serviceMock = new Mock<IDeskService>();
			var desks = new List<Desk>
			{
				new Desk { DeskId = Guid.NewGuid() },
				new Desk { DeskId = Guid.NewGuid() }
			};

			serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(desks);

			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Get();
			var resultStatusCode = result.Result as OkObjectResult;

			Assert.IsNotNull(resultStatusCode);
			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<IEnumerable<Desk>>(resultStatusCode.Value);
		}

		[Test]
		public async Task Get_ReturnsNotFoundWhenNoDesks()
		{
			var serviceMock = new Mock<IDeskService>();
			serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync((IEnumerable<Desk>)null);

			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Get();
			var resultStatusCode = result.Result as NotFoundResult;

			Assert.IsInstanceOf<NotFoundResult>(result.Result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Get_ReturnsOkObjectResultWhenDeskExists()
		{
			var id = Guid.NewGuid();
			var desk = new DeskDto { DeskNumber = 1, IsOccupied = true, IsStatic = false };

			var mockService = new Mock<IDeskService>();
			mockService.Setup(service => service.GetOneAsync(id))
				.ReturnsAsync(desk);

			var controller = new DesksController(mockService.Object);

			var result = await controller.Get(id);
			var okObjectResult = result.Result as OkObjectResult;

			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<DeskDto>(okObjectResult.Value);
		}

		[Test]
		public async Task Get_ReturnsNotFoundResultWhenDeskDoesNotExist()
		{
			var id = Guid.NewGuid();
			var mockService = new Mock<IDeskService>();
			mockService.Setup(service => service.GetOneAsync(id)).ReturnsAsync((DeskDto)null);

			var controller = new DesksController(mockService.Object);

			var result = await controller.Get(id);
			var resultStatusCode = result.Result as NotFoundResult;

			Assert.IsInstanceOf<NotFoundResult>(result.Result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Put_ReturnsOkResultOnSuccess()
		{
			var id = Guid.NewGuid();
			var deskMock = new DeskDto { DeskNumber = 1, IsOccupied = true, IsStatic = false };

			var mockService = new Mock<IDeskService>();
			mockService.Setup(service => service.UpdateOneAsync(id, deskMock))
				.ReturnsAsync(true);

			var controller = new DesksController(mockService.Object);

			var result = await controller.Put(id, deskMock);
			var resultStatusCode = result as OkObjectResult;

			Assert.IsInstanceOf<OkObjectResult>(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
		}

		[Test]
		public async Task Put_ReturnsInternalServerErrorOnFail()
		{
			var id = Guid.NewGuid();
			var deskDto = (DeskDto)null;

			var serviceMock = new Mock<IDeskService>();
			serviceMock.Setup(service => service.UpdateOneAsync(id, deskDto)).ReturnsAsync(false);

			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Put(id, deskDto) as StatusCodeResult;

			Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Post_ReturnsCreatedResultOnSuccess()
		{
			var deskDto = new DeskDto { DeskNumber = 1, IsOccupied = true, IsStatic = false };
			var serviceMock = new Mock<IDeskService>();

			serviceMock.Setup(service => service.CreateOneAsync(deskDto)).ReturnsAsync(true);
			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Post(deskDto);
			var resultStatusCode = result.Result as StatusCodeResult;

			Assert.IsNotNull(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
		}

		[Test]
		public async Task Post_ReturnsStatusCodeCreatedOnFail()
		{
			var deskDto = new DeskDto { DeskNumber = 1, IsOccupied = true, IsStatic = false };
			var serviceMock = new Mock<IDeskService>();

			serviceMock.Setup(service => service.CreateOneAsync(deskDto)).ReturnsAsync(false);
			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Post(deskDto);
			var resultStatusCode = result.Result as StatusCodeResult;

			Assert.IsNotNull(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
		}

		[Test]
		public async Task Delete_ReturnsNoContentResultOnSuccess()
		{
			var id = Guid.NewGuid();
			var serviceMock = new Mock<IDeskService>();
			serviceMock.Setup(service => service.DeleteOneAsync(id)).ReturnsAsync(true);
			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Delete(id);

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<NoContentResult>(result);
		}

		[Test]
		public async Task Delete_ReturnsNotFoundResultOnFail()
		{
			var id = Guid.NewGuid();

			var serviceMock = new Mock<IDeskService>();
			serviceMock.Setup(service => service.DeleteOneAsync(id)).ReturnsAsync(false);

			var controller = new DesksController(serviceMock.Object);

			var result = await controller.Delete(id);
			var resultStatusCode = result as StatusCodeResult;

			Assert.IsNotNull(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));

		}
	}
}




