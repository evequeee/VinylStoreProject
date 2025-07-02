using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;
using ProjectVinylStore.DataAccess.Interfaces;

namespace ProjectVinylStore.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser?> GetUserWithOrdersAsync(string userId)
        {
            return await _context.Users
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}