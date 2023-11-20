using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Net.Mime;
using DeskReserve.Exceptions;
using DeskReserve.Interfaces;
using RequestReserve.Interfaces;

namespace DeskReserve.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class RequestController : ControllerBase
	{
		private readonly IRequestService _service;

		public RequestController(IRequestService service)
		{
			_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<Request>>> Get()
		{
			var requests = await _service.GetAllAsync();
			return Ok(requests);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Desk))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<RequestDto>> Get(Guid id)
		{
			ActionResult result;

			try
			{
				var request = await _service.GetAsync(id);
				result = Ok(request);
			}
			catch (EntityNotFoundException ex)
			{
				result = NotFound(ex);
			}

			return result;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<ActionResult> Create(RequestDto requestDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var success = await _service.CreateAsync(requestDto);

			return success ? StatusCode(StatusCodes.Status201Created) : StatusCode(StatusCodes.Status409Conflict);
		}

		[HttpPut("{id}/approve")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Approve(Guid id, [FromBody] RequestDto requestDto)
		{
			ActionResult result; 

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var success = await _service.ApproveAsync(id, requestDto);
				result = Ok(success);
			}
			catch (EntityNotFoundException ex)
			{
				result = NotFound(ex);
			}

			return result;
		}
	}

}
