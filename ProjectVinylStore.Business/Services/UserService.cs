using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Services;
using ProjectVinylStore.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<AuthResultDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "User with this email already exists"
                };
            }

            // Create new user using ApplicationUser
            var user = new ApplicationUser
            {
                FirstName = registerDto.Name.Split(' ').FirstOrDefault() ?? registerDto.Name,
                LastName = registerDto.Name.Split(' ').Skip(1).FirstOrDefault() ?? "",
                Email = registerDto.Email,
                UserName = registerDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResultDto
            {
                IsSuccess = true,
                Message = "User registered successfully",
                User = new UserDto
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email ?? string.Empty
                }
            };
        }

        public async Task<AuthResultDto> LoginUserAsync(LoginUserDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            return new AuthResultDto
            {
                IsSuccess = true,
                Message = "Login successful",
                User = new UserDto
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email ?? string.Empty
                }
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            // Use pattern matching and fix the type mismatch issue
            if (await _unitOfWork.Users.GetByIdAsync(int.Parse(userId)) is ApplicationUser user)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email ?? string.Empty
                };
            }

            return null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email ?? string.Empty
            };
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null && await _userManager.CheckPasswordAsync(user, password);
        }
    }
}