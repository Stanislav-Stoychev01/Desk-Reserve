using Microsoft.AspNetCore.Mvc;
using Moq;
using DeskReserve.Controllers;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.AspNetCore.Http;
using DeskReserve.Exceptions;
using RequestReserve.Interfaces;

namespace DeskReserve_Tests
{
	[TestFixture]
	public class RequestControllerTests
	{
		private RequestController _controller;
		private Mock<IRequestService> _serviceMock;

		[SetUp]
		public void Setup()
		{
			_serviceMock = new Mock<IRequestService>();
			_controller = new RequestController(_serviceMock.Object);
		}

		[Test]
		public async Task Get_ReturnsOkWithRequests()
		{
			var desks = new List<Request>
			{
				new Request { RequestId = Guid.NewGuid() },
				new Request { RequestId = Guid.NewGuid() }
			};

			_serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(desks);

			var result = await _controller.Get();
			var resultStatusCode = result.Result as OkObjectResult;

			Assert.IsNotNull(resultStatusCode);
			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<IEnumerable<Request>>(resultStatusCode.Value);
		}

		[Test]
		public async Task Get_ReturnsNotFoundWhenNoRequests()
		{
			_serviceMock.Setup(service => service.GetAllAsync()).ReturnsAsync((IEnumerable<Request>)null);

			var result = await _controller.Get();
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

			_serviceMock.Setup(service => service.GetAsync(id))
				.ReturnsAsync(desk);

			var result = await _controller.Get(id);
			var okObjectResult = result.Result as OkObjectResult;

			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<RequestDto>(okObjectResult.Value);
		}

		[Test]
		public async Task Get_ReturnsNotFoundResultWhenRequestDoesNotExist()
		{
			var id = Guid.NewGuid();

			_serviceMock.Setup(service => service.GetAsync(id)).ThrowsAsync(new EntityNotFoundException());

			var result = await _controller.Get(id);
			var resultStatusCode = result.Result as NotFoundObjectResult;

			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Put_WithValidRequestDto_ReturnsCreatedResult()
		{
			var requestDto = new RequestDto {
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			_serviceMock.Setup(service => service.CreateAsync(requestDto))
				.ReturnsAsync(true);

			var result = await _controller.Post(requestDto);

			var resultStatusCode = result.Result as StatusCodeResult;

			Assert.IsNotNull(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
		}

		[Test]
		public async Task Post_ReturnsStatusCodeCreatedOnFail()
		{
			var requestDto = new RequestDto {
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			_serviceMock.Setup(service => service.CreateAsync(requestDto)).ReturnsAsync(false);

			var result = await _controller.Post(requestDto);
			var resultStatusCode = result.Result as StatusCodeResult;

			Assert.IsNotNull(result);
			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
		}

		[Test]
		public async Task Post_ReturnsNotFoundResultWhenDeskDoesNotExist()
		{
			var requestDto = new RequestDto
			{
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			_serviceMock.Setup(service => service.CreateAsync(requestDto)).ThrowsAsync(new EntityNotFoundException());

			var result = await _controller.Post(requestDto);
			var resultStatusCode = result.Result as NotFoundObjectResult;

			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));

		}

		[Test]
		public async Task Patch_ValidIdAndStateUpdate_ReturnsOk()
		{
			Guid id = Guid.NewGuid();
			var stateUpdateDto = new StateUpdateDto {
				NewState = DeskReserve.Utils.BookingState.Approved
			};
			var updatedRequest = new RequestDto {
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			_serviceMock.Setup(s => s.UpdateAsync(id, stateUpdateDto)).ReturnsAsync(updatedRequest);

			var result = await _controller.Patch(id, stateUpdateDto);
			var okObjectResult = result.Result as OkObjectResult;

			Assert.IsInstanceOf<OkObjectResult>(result.Result);
			Assert.IsInstanceOf<RequestDto>(okObjectResult.Value);
		}

		[Test]
		public async Task Patch_EntityNotFound_ReturnsNotFound()
		{
			Guid id = Guid.NewGuid();
			var stateUpdateDto = new StateUpdateDto { NewState = DeskReserve.Utils.BookingState.Approved };
			_serviceMock.Setup(s => s.UpdateAsync(id, stateUpdateDto)).ThrowsAsync(new EntityNotFoundException("Entity not found"));

			var result = await _controller.Patch(id, stateUpdateDto);
			var resultStatusCode = result.Result as NotFoundObjectResult;

			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task Patch_InvalidStateException_ReturnsBadRequest()
		{
			// Arrange
			Guid id = Guid.NewGuid();
			var stateUpdateDto = new StateUpdateDto { NewState = DeskReserve.Utils.BookingState.Requested };
			_serviceMock.Setup(s => s.UpdateAsync(id, stateUpdateDto)).ThrowsAsync(new InvalidStateException("Invalid state"));

			var result = await _controller.Patch(id, stateUpdateDto);
			var resultStatusCode = result.Result as BadRequestObjectResult;

			Assert.That(resultStatusCode.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
		}

	}
}

