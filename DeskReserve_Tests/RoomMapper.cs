using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using RoomReserve.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskReserve_Tests
{
	internal class RoomMapper
	{
		[Test]
		public void ToRoomDto_ConvertsRoomToRoomDto()
		{
			Guid roomId = Guid.NewGuid();

			Room room = new Room
			{
				RoomId = roomId,
				RoomNumber = 1,
				HasAirConditioner = true,
			};

			RoomDto roomDto = room.ToRoomDto();

			Assert.AreEqual(room.RoomNumber, roomDto.RoomNumber);
			Assert.AreEqual(room.HasAirConditioner, roomDto.HasAirConditioner);
		}

		[Test]
		public void ToRoom_ConvertsRoomDtoToRoom()
		{
			RoomDto roomDto = new RoomDto
			{
				RoomNumber = 1,
				HasAirConditioner = true
			};

			Room room = roomDto.ToRoom();

			Assert.AreEqual(room.RoomNumber, roomDto.RoomNumber);
			Assert.AreEqual(room.HasAirConditioner, roomDto.HasAirConditioner);
		}

		/*public static Room UpdateFromDto(this Room Room, RoomDto RoomDto)
		{
			Room.RoomNumber = RoomDto.RoomNumber;
			Room.HasAirConditioner = RoomDto.HasAirConditioner;
			return Room;
		}*/

		[Test]
		public void UpdateRoomFromDto()
		{
			RoomDto roomDto = new RoomDto
			{
				RoomNumber = 1,
				HasAirConditioner = true
			};

			Guid roomId = Guid.NewGuid();

			Room room = new Room
			{
				RoomId = roomId,
				RoomNumber = 2,
				HasAirConditioner = false,
			};

			room.UpdateFromDto(roomDto);

			Assert.AreEqual(room.RoomNumber, roomDto.RoomNumber);
			Assert.AreEqual(room.HasAirConditioner, roomDto.HasAirConditioner);

		}
	}
}
