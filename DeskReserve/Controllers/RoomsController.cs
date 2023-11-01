using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Net.Mime;
using System.Drawing;

namespace DeskReserve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase, IRoomController
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
			_roomService = roomService ?? throw new ArgumentNullException(nameof(_roomService));
		}

        // GET: api/Rooms
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        {
            var rooms = await _roomService.GetAllAsync();
            return Ok(rooms);
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetById(Guid id)
        {
            var room = await _roomService.GetOneAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

		// PUT: api/Rooms/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("edit/{id}")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateOne(Guid id, Room room)
        {
			var success = await _roomService.UpdateOneAsync(id, room);

			return success ? NoContent() : NotFound();
		}

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new")]
        public async Task<ActionResult<Room>> Create(Room room)
        {
			var newRoom = await _roomService.CreateOneAsync(room);
            return Ok(newRoom);//StatusCode(StatusCodes.Status201Created, newRoom);
		}

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid id)
        {
			var success = await _roomService.DeleteOneAsync(id);

			return success ? NoContent() : NotFound();
		}

        /*private bool RoomExists(Guid id)
        {
            return _roomService.Rooms.Any(e => e.RoomId == id);
        }*/
    }
}
