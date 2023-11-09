using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Domain.Service
{
    public interface IDeskService
    {
        Task<IEnumerable<Desk>> GetAllAsync();

        Task<DeskDto> GetOneAsync(Guid id);

        Task<bool> UpdateOneAsync(Guid id, DeskDto deskDto);

        Task<bool> CreateOne(DeskDto desk);

        Task<bool> DeleteOneAsync(Guid id);

    }
}
