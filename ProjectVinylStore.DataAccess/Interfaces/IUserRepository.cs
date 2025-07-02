using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetUserWithOrdersAsync(string userId);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
    }
}