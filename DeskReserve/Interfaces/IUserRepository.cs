using DeskReserve.Data.DBContext.Entity;

namespace DeskReserve.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(Guid id);

        Task<Role> GetRoleById(Guid userId);

        Task<User> GetByEmail(string email);

        Task<bool> Add(User user);

        Task<bool> Update(User user);

        Task<bool> Delete(Guid id);
    }
}
