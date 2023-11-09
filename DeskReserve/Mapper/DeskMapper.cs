﻿using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

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

		public static Desk UpdateFromDto(this Desk desk, DeskDto deskDto)
		{
			desk.DeskNumber = deskDto.DeskNumber;
			desk.IsOccupied = deskDto.IsOccupied;
			desk.IsStatic = deskDto.IsStatic;

			return desk;
		}
	}
}