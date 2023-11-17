using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Utils;
using System.ComponentModel.DataAnnotations;

namespace DeskReserve.Mapper
{
	public static class RequestMapper
	{
		public static Request ToRequest(this RequestDto requestDto)
		{
			var request = new Request
			{
				OccupationStatus = requestDto.OccupationStatus,
				State = requestDto.State,
				DeskId = requestDto.DeskId
			};

			return request;
		}

		public static Request ApproveRequest(this RequestDto requestDto)
		{
			var request = requestDto.ToRequest();
			

			

			return request;
		}
	}
}





