﻿using DeskReserve.Data.DBContext.Entity;
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

		public async Task<bool> ApproveAsync(Guid id, RequestDto requestDto)
		{
			Request request = await _repository.GetById(id) ?? throw new EntityNotFoundException();

			request.ApproveFromDto(requestDto);

			return await _repository.Approve(request);
		}

		public async Task<bool> CreateAsync(RequestDto requestDto)
		{
			Request request = requestDto.ToRequest();

			var validationContext = new ValidationContext(request, serviceProvider: null, items: null);
			Validator.ValidateObject(request, validationContext, validateAllProperties: true);

			return await _repository.Create(request);
		}
	}

}