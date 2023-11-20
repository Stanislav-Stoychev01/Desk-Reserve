using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
namespace RoomReserve.Mapper
{
	public static class RoomMapper
	{
		public static RoomDto ToRoomDto(this Room Room)
		{
			return new RoomDto
			{
				RoomNumber = Room.RoomNumber,
				HasAirConditioner = Room.HasAirConditioner
			};
		}

		public static Room ToRoom(this RoomDto RoomDto)
		{
			return new Room
			{
				RoomNumber = RoomDto.RoomNumber,
				HasAirConditioner = RoomDto.HasAirConditioner
			};
		}

		public static Room UpdateFromDto(this Room Room, RoomDto RoomDto)
		{
			Room.RoomNumber = RoomDto.RoomNumber;
			Room.HasAirConditioner = RoomDto.HasAirConditioner;
			return Room;
		}

	}
}
