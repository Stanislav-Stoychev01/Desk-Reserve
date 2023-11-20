using Microsoft.AspNetCore.Mvc;
using Moq;
using DeskReserve.Controllers;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Http;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using DeskReserve.Services;
using RequestReserve.Interfaces;

namespace DeskReserve_Tests
{
	[TestFixture]
	public class RequestControllerTests
	{
		[Test]
		public async Task Get_ReturnsOkWithRequests()
		{
			var serviceMock = new Mock<IRequestService>();
			var desks = new List<Request>
			{
				new Request { RequestId = Guid.NewGuid() },
				new Request { RequestId = Guid.NewGuid() }
			};

			serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(desks);

			var controller = new RequestController(serviceMock.Object);

			var result = await controller.Get();
			var resultStatusCode = result.Result as OkObjectResult;

			Assert.IsNotNull(resultStatusCode);
			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<IEnumerable<Request>>(resultStatusCode.Value);
		}

		[Test]
		public async Task Get_ReturnsNotFoundWhenNoRequests()
		{
			var serviceMock = new Mock<IRequestService>();
			serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync((IEnumerable<Request>)null);

			var controller = new RequestController(serviceMock.Object);

			var result = await controller.Get();
			var resultStatusCode = result.Result as NotFoundResult;

			Assert.IsInstanceOf<NotFoundResult>(result.Result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Get_ReturnsOkObjectResultWhenRequestExists()
		{
			var id = Guid.NewGuid();
			var desk = new RequestDto
			{
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00), 
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			var mockService = new Mock<IRequestService>();
			mockService.Setup(service => service.GetAsync(id))
				.ReturnsAsync(desk);

			var controller = new RequestController(mockService.Object);

			var result = await controller.Get(id);
			var okObjectResult = result.Result as OkObjectResult;

			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<RequestDto>(okObjectResult.Value);
		}

		[Test]
		public async Task Get_ReturnsNotFoundResultWhenRequestDoesNotExist()
		{
			var id = Guid.NewGuid();
			var mockService = new Mock<IRequestService>();
			mockService.Setup(service => service.GetAsync(id)).ThrowsAsync(new EntityNotFoundException());

			var controller = new RequestController(mockService.Object);

			var result = await controller.Get(id);
			var resultStatusCode = result.Result as NotFoundObjectResult;

			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Create_WithValidRequestDto_ReturnsCreatedResult()
		{
			var requestDto = new RequestDto {
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			var mockService = new Mock<IRequestService>();
			mockService.Setup(service => service.CreateAsync(requestDto))
				.ReturnsAsync(true);

			var controller = new RequestController(mockService.Object);

			var result = await controller.Post(requestDto);

			var resultStatusCode = result.Result as StatusCodeResult;

			Assert.IsNotNull(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
		}
	}
}
