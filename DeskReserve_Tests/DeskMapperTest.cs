using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using DeskReserve.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desk_Reserve.Tests
{
	[TestFixture]
	public class DeskMapperTests
	{
		[Test]
		public void ToDeskDto_ConvertsDeskToDeskDto()
		{
			Desk desk = new Desk
			{
				DeskNumber = 1,
				IsOccupied = true,
				IsStatic = false
			};

			DeskDto deskDto = desk.ToDeskDto();

			Assert.AreEqual(desk.DeskNumber, deskDto.DeskNumber);
			Assert.AreEqual(desk.IsOccupied, deskDto.IsOccupied);
			Assert.AreEqual(desk.IsStatic, deskDto.IsStatic);
		}

		[Test]
		public void ToDesk_ConvertsDeskDtoToDesk()
		{
			DeskDto deskDto = new DeskDto
			{
				DeskNumber = 1,
				IsOccupied = true,
				IsStatic = false
			};

			Desk desk = deskDto.ToDesk();

			Assert.AreEqual(deskDto.DeskNumber, desk.DeskNumber);
			Assert.AreEqual(deskDto.IsOccupied, desk.IsOccupied);
			Assert.AreEqual(deskDto.IsStatic, desk.IsStatic);
		}
	}
}
