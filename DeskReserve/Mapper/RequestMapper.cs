using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Utils;
using System.ComponentModel.DataAnnotations;

namespace DeskReserve.Mapper
{
	public static class RequestMapper
	{
		public static Request ApproveFromDto( this  Request request, RequestDto requestDto)
		{
			//request.OccupationStatus = requestDto.OccupationStatus;
			request.State = requestDto.State;
			//request.DeskId = requestDto.DeskId;

			return request;
		}

		public static RequestDto ToRequestDto(this Request request)
		{
			return new RequestDto
			{
				RequestStartDate = request.RequestStartDate,
				RequestEndDate = request.RequestEndDate,
				OccupationStatus = request.OccupationStatus,
				State = request.State,
				DeskId = request.DeskId
			};
		}

		public static Request ToRequest(this RequestDto requestDto)
		{
			return new Request
			{
				RequestStartDate = requestDto.RequestStartDate,
				RequestEndDate = requestDto.RequestEndDate,
				OccupationStatus = requestDto.OccupationStatus,
				State = requestDto.State,
				DeskId = requestDto.DeskId
			};
		}
	}
}





