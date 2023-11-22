using Microsoft.AspNetCore.Mvc;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using System.Net.Mime;
using DeskReserve.Exceptions;
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

			return !ReferenceEquals(requests, null) ? Ok(requests) : NotFound();
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
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Request>> Post(RequestDto requestDto)
		{
			ActionResult result;

			try
			{
				var success = await _service.CreateAsync(requestDto);

				if (success)
				{
					result = StatusCode(201);
				}
				else
				{
					result = StatusCode(409);
				}
			}
			catch (Exception ex)
			{
				result = NotFound(ex.Message);
			}

			return result;
		}

		[HttpPut("{id}/approve")]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Approve(Guid id, [FromBody] RequestDto requestDto)
		{
			ActionResult result; 

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
