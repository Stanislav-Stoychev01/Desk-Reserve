//using DeskReserve.Data.DBrepository;
//using DeskReserve.Data.DBrepository.Entity;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using DeskReserve.Exceptions;
using DeskReserve.Repository;
using DeskReserve.Data.DBContext.Entity;
using RoomReserve.Mapper;

namespace DeskReserve.Domain
{
	public class RoomService : IRoomService
	{
		private readonly IRoomRepository _repository;

		public RoomService(IRoomRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		//public async Task<List<Room>> GetRooms()
		public async Task<IEnumerable<Room>> GetAll()
		{
			return await _repository.GetAllAsync();
		}

		//public async Task<Room> GetRoom(Guid id)
		public async Task<RoomDto> Get(Guid id)
		{
			var room = await _repository.GetOneAsync(id);

			return room.ToRoomDto();

		}

		public async Task<bool> Update(Guid id, RoomDto roomDto)
		{
			var room = await _repository.GetOneAsync(id) ?? throw new EntityNotFoundException();
			room.UpdateFromDto(roomDto);
			return await _repository.UpdateOneAsync(room);
		}

		public async Task<bool> Create(RoomDto roomDto)
		{
			Room room = roomDto.ToRoom();
			return await _repository.CreateOneAsync(room);
		}

		public async Task<bool> Delete(Guid id)
		{
			return await _repository.DeleteOneAsync(id);

		}
	}
}
