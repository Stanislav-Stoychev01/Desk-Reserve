﻿using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Drawing;

namespace DeskReserve.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAll();
        Task<RoomDto> Get(Guid id);
        Task<bool> Update(Guid id, RoomDto room);
        Task<bool> Create(RoomDto roomDto);
        Task<bool> Delete(Guid id);
    }
}
