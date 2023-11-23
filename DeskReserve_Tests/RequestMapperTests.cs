using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Mapper;


namespace DeskReserve_Tests
{
	[TestFixture]
	internal class RequestMapperTests
	{
		[Test]
		public void ToRequestDto_ConvertRequestToRequestDto()
		{
			Request request = new Request
			{
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested

			};

			RequestDto requestDto = request.ToRequestDto();

			Assert.AreEqual(request.RequestStartDate, requestDto.RequestStartDate);
			Assert.AreEqual(request.RequestEndDate, requestDto.RequestEndDate);
			Assert.AreEqual(request.DeskId, requestDto.DeskId);
			Assert.AreEqual(request.OccupationStatus, requestDto.OccupationStatus);
			Assert.AreEqual(request.State, requestDto.State);
		}

		[Test]
		public void ToDesk_ConvertsDeskDtoToDesk()
		{
			RequestDto requestDto = new RequestDto
			{
				RequestStartDate = new DateTime(2023, 11, 20, 15, 48, 54),
				RequestEndDate = new DateTime(2023, 11, 21, 16, 00, 00, 00),
				DeskId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
				OccupationStatus = DeskReserve.Utils.OccupationStatus.Temporary,
				State = DeskReserve.Utils.BookingState.Requested
			};

			Request request = requestDto.ToRequest();

			Assert.AreEqual(requestDto.RequestStartDate, request.RequestStartDate);
			Assert.AreEqual(requestDto.RequestEndDate, request.RequestEndDate);
			Assert.AreEqual(requestDto.DeskId, request.DeskId);
			Assert.AreEqual(requestDto.OccupationStatus, request.OccupationStatus);
			Assert.AreEqual(requestDto.State, request.State);
		}
	}
}
