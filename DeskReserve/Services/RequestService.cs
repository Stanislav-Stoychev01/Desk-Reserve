using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using DeskReserve.Mapper;
using DeskReserve.Exceptions;
using DeskReserve.Domain;
using DeskReserve.Interfaces;
using RequestReserve.Interfaces;
using DeskReserve.Utils;
using System.ComponentModel.DataAnnotations;

namespace DeskReserve.Services
{
	public class RequestService : IRequestService
	{
		private readonly IRequestRepository _repository;

		public RequestService(IRequestRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<Request>> GetAllAsync()
		{
			return await _repository.GetAll();
		}

		public async Task<RequestDto> GetAsync(Guid id)
		{
			var request = await _repository.GetById(id);

			return request.ToRequestDto();
		}

		public async Task<bool> CreateAsync(RequestDto requestDto)
		{
			Request request = requestDto.ToRequest();

			var validationContext = new ValidationContext(request, serviceProvider: null, items: null);
			Validator.ValidateObject(request, validationContext, validateAllProperties: true);

			return await _repository.Create(request);
		}

		public async Task<RequestDto> UpdateAsync(Guid id, StateUpdateDto stateUpdateDto)
		{
			var request = await _repository.GetById(id) ?? throw new EntityNotFoundException("Request not found");

			var validationContext = new ValidationContext(request, serviceProvider: null, items: null);
			Validator.ValidateObject(request, validationContext, validateAllProperties: true);


			if (stateUpdateDto.NewState != BookingState.Rejected || stateUpdateDto.NewState != BookingState.Approved)
			{
				throw new InvalidStateException("New state is invalid. It has to be appoved or rejected");
			}

			request.State = stateUpdateDto.NewState;

			await _repository.Uppdate(request);

			return request.ToRequestDto();
		}
	}

}

