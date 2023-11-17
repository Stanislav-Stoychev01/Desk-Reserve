using Microsoft.EntityFrameworkCore;
using System.Drawing;
using DeskReserve.Exceptions;
using DeskReserve.Data.DBContext.Entity;
using RoomReserve.Mapper;
using DeskReserve.Interfaces;
using DeskReserve.Domain;

namespace DeskReserve.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;

        public RoomService(IRoomRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<RoomDto> Get(Guid id)
        {
            var room = await _repository.GetById(id);

            return room.ToRoomDto();

        }

        public async Task<bool> Update(Guid id, RoomDto roomDto)
        {
            var room = await _repository.GetById(id) ?? throw new EntityNotFoundException("Not found entity");
            room.UpdateFromDto(roomDto);
            return await _repository.Update(room);
        }

        public async Task<bool> Create(RoomDto roomDto)
        {
            Room room = roomDto.ToRoom();
            return await _repository.Create(room);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.Delete(id);

        }
    }
}
