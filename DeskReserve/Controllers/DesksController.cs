using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Net.Mime;

namespace DeskReserve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesksController : ControllerBase, IDeskController
    {
        private readonly IDeskService _service;

		public DesksController(IDeskService service)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		[HttpGet]
        public async Task<ActionResult<IEnumerable<Desk>>> GetAll()
        {
            var desks = await _service.GetAllAsync();
			return Ok(desks);
        }

        [HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Desk))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Desk>> GetById(Guid id)
        {
            var desk = await _service.GetOneAsync(id);

			return desk == null ? NotFound() : Ok(desk);
		}

		[HttpPut("{id}")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Update(Guid id, Desk desk)
		{
			if (id != desk.DeskId)
			{
				return BadRequest();
			}

			var success = await _service.UpdateOneAsync(id, desk);

			return success ? NoContent() : NotFound();
		}

		[HttpPost]
		public async Task<ActionResult<Desk>> Create(Desk desk)
		{
			var newDesk = await _service.CreateOneAsync(desk);

			return Ok(newDesk);
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete(Guid id)
		{
			var success = await _service.DeleteOneAsync(id);

			return success ? NoContent() : NotFound();
		}

	}
}
