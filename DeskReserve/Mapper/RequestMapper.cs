using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Mapper
{
	public static class RequestMapper
	{
		public static RequestDto ToRequestDto(this Request request)
		{
			return new RequestDto
			{
				RequestStartDate = request.RequestStartDate,
				RequestEndDate = request.RequestEndDate,
				State = request.State,
				isPermanentlyOccupied = request.isPermanentlyOccupied,
				DeskId = request.DeskId
			};
		}

		public static Request ToRequest(this RequestDto requestDto)
		{
			return new Request
			{
				isPermanentlyOccupied = requestDto.isPermanentlyOccupied,
				RequestStartDate = requestDto.isPermanentlyOccupied ? DateTime.MinValue : requestDto.RequestStartDate,
				RequestEndDate = requestDto.isPermanentlyOccupied ? DateTime.MinValue : requestDto.RequestEndDate,
				State = requestDto.State,
				DeskId = requestDto.DeskId
			};
		}

		public static Request UpdateFromDto(this Request request, RequestDto requestDto)
		{
			request.State = requestDto.State;

			return request;
		}
	}
}
