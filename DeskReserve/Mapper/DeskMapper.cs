using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Mapper
{
	public static class DeskMapper
	{
		public static DeskDto ToDeskDto(this Desk desk)
		{
			return new DeskDto
			{
				DeskNumber = desk.DeskNumber,
				IsOccupied = desk.IsOccupied,
				IsStatic = desk.IsStatic
			};
		}

		public static Desk ToDesk(this DeskDto deskDto)
		{
			return new Desk
			{
				DeskNumber = deskDto.DeskNumber,
				IsOccupied = deskDto.IsOccupied,
				IsStatic = deskDto.IsStatic
			};
		}
	}
}
