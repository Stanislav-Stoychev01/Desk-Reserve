using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Repository;
using NuGet.Protocol.Core.Types;
using DeskReserve.Mapper;
using DeskReserve.Exceptions;
using DeskReserve.Domain;

namespace DeskReserve.Domain.Service
{
	public class DeskService : IDeskService
	{
		private readonly IDeskRepository _repository;

		public DeskService(IDeskRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IEnumerable<Desk>> GetAllAsync()
		{
			return await _repository.GetAll();
		}

		public async Task<DeskDto> GetAsync(Guid id)
		{
			var desk = await _repository.GetById(id);

			return desk.ToDeskDto();
		}

		public async Task<bool> UpdateAsync(Guid id, DeskDto deskDto)
		{
			var desk = await _repository.GetById(id) ?? throw new EntityNotFoundException();

			desk.UpdateFromDto(deskDto);

			return await _repository.Update(desk);
		}

		public async Task<bool> CreateAsync(DeskDto deskDto)
		{
			Desk desk = deskDto.ToDesk();

			return await _repository.Create(desk);
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await _repository.Delete(id);
		}
	}

}