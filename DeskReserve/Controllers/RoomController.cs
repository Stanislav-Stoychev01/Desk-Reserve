﻿using System;
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
using DeskReserve.Exceptions;

namespace DeskReserve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
			_roomService = roomService ?? throw new ArgumentNullException(nameof(_roomService));
		}
      
        [HttpGet("list")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<Room>>> GetAll()
        {
            var rooms = await _roomService.GetAll();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Room>> GetById(Guid id)
        {
            ActionResult result;
			try
			{
				var room = await _roomService.Get(id);
				result = Ok(room);
			}
			catch (EntityNotFoundException ex)
			{
				result = NotFound(ex);
			}

            return result;
        }

		[HttpPut("edit/{id}")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> UpdateOne(Guid id, RoomDto room)
        {
			IActionResult result;

			try
			{
				var success = await _roomService.Update(id, room);
				result = Ok(success);
			}
			catch (EntityNotFoundException ex)
			{
				result = NotFound(ex);
			}

			return result;
		}

        [HttpPost("new")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<Room>> CreateAsync(RoomDto roomDto)
        {
			var success = await _roomService.Create(roomDto);

            return success ? StatusCode(StatusCodes.Status201Created) : StatusCode(StatusCodes.Status409Conflict);
		}

        [HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid id)
        {
			IActionResult response;

			try
			{
				var success = await _roomService.Delete(id);
				response = NoContent();
			}
			catch (EntityNotFoundException ex)
			{
				response = NotFound(ex);
			}

			return response;
		}

    }
}
