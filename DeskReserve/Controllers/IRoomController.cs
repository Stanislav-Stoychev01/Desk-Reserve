using DeskReserve.Data.DBContext.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace DeskReserve.Controllers
{
	public interface IRoomController
	{
		Task<ActionResult<IEnumerable<Room>>> GetAll();
		Task<ActionResult<Room>> GetById(Guid id);

		Task<IActionResult> UpdateOne(Guid id, Room floor);

		Task<ActionResult<Room>> Create(Room floor);

		Task<IActionResult> Delete(Guid id);
	}
}
