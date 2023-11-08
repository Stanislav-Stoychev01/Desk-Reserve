using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Repository;
using NuGet.Protocol.Core.Types;
using DeskReserve.Mapper;
using DeskReserve.Exception;

namespace DeskReserve.Domain
{
	public class DeskService : IDeskService
	{
		private readonly IDeskRepository repository;

		public DeskService(IDeskRepository _repository)
		{
			repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
		}

		public async Task<IEnumerable<Desk>> GetAllAsync()
		{
			return await repository.GetAll();
		}

		public async Task<DeskDto> GetOneAsync(Guid id)
		{
			var desk = await repository.GetById(id) ?? throw new EntityNotFoundException();
		
			return desk.ToDeskDto();
		}

		public async Task<bool> UpdateOneAsync(Guid id, DeskDto deskDto)
		{
			var desk = await repository.GetById(id) ?? throw new EntityNotFoundException();

			desk.DeskNumber = deskDto.DeskNumber;
			desk.IsOccupied = deskDto.IsOccupied;
			desk.IsStatic = deskDto.IsStatic;

			return await repository.Update(desk);
		}


		public async Task<bool> CreateOneAsync(DeskDto deskDto)
		{
			Desk desk = deskDto.ToDesk();

			return await repository.Create(desk);
		}

		public async Task<bool> DeleteOneAsync(Guid id)
		{
			return await repository.Delete(id);
		}
	}

}