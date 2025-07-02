using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<bool> LogoutAsync(string userId);
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
    }
}