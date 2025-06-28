using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserWithOrdersAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
    }
}