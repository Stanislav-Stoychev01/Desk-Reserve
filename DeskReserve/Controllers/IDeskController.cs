using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Controllers
{
	public interface IDeskController
	{
		Task<ActionResult<IEnumerable<Desk>>> GetAll();

		Task<ActionResult<Desk>> GetById(Guid id);

		Task<IActionResult> Update(Guid id, Desk desk);

		Task<ActionResult<Desk>> Create(Desk desk);

		Task<IActionResult> Delete(Guid id);
	}
}
