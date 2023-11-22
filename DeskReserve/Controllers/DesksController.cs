using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Net.Mime;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;

namespace DeskReserve.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class DesksController : ControllerBase
	{
		private readonly IDeskService _service;

		public DesksController(IDeskService service)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<Desk>>> Get()
		{
			var desks = await _service.GetAllAsync();

			return !ReferenceEquals(desks, null) ? Ok(desks) : NotFound();
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Desk))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Desk>> Get(Guid id)
		{
			ActionResult result;

			try
			{
				var desk = await _service.GetAsync(id);
				result = Ok(desk);
			}
			catch (EntityNotFoundException ex)
			{
				result = NotFound(ex.Message);
			}

			return result;
		}

		[HttpPut("{id}")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Put(Guid id, DeskDto desk)
		{
			IActionResult result;

			try
			{
				var success = await _service.UpdateAsync(id, desk);
				result = Ok(success);
			}
			catch (EntityNotFoundException ex)
			{
				result = NotFound(ex.Message);
			}

			return result;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult<Desk>> Post(DeskDto desk)
		{
			var success = await _service.CreateAsync(desk);

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
				var success = await _service.DeleteAsync(id);
				response = NoContent();
			}
			catch (EntityNotFoundException ex)
			{
				response = NotFound(ex.Message);
			}

			return response;
		}

	}
}