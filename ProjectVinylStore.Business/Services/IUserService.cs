using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Services
{
    public interface IUserService
    {
        // User Registration & Login
        Task<AuthResultDto> RegisterUserAsync(RegisterUserDto registerDto);
        Task<AuthResultDto> LoginUserAsync(LoginUserDto loginDto);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
    }
}