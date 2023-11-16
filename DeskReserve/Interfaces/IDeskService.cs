using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;

namespace DeskReserve.Interfaces
{
    public interface IDeskService
    {
        Task<IEnumerable<Desk>> GetAllAsync();

        Task<DeskDto> GetAsync(Guid id);

        Task<bool> UpdateAsync(Guid id, DeskDto deskDto);

        Task<bool> CreateAsync(DeskDto desk);

        Task<bool> DeleteAsync(Guid id);

    }
}