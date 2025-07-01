using ProjectVinylStore.DataAccess.Interfaces;
using ProjectVinylStore.DataAccess.Entities;
using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Services;

namespace ProjectVinylStore.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResultDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            //cheking if user already exists
            var existingUser = await _unitOfWork.Users.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "User already exists."
                };
            }

            //creating a new user
            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password, // need to hash the password in production
                Orders = new List<Order>()
            };

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CommitAsync();

            return new AuthResultDto
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                User = new UserDto
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    Email = newUser.Email
                }
            };
        }

        public async Task<AuthResultDto> LoginUserAsync(LoginUserDto loginDto)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(loginDto.Email);

            if (user == null || user.Password != loginDto.Password) // also need to hash the password in production
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            return new AuthResultDto()
            {
                IsSuccess = true,
                Message = "Login successful.",
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                },
            };
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.GetUserByEmailAsync(email);
            return user != null && user.Password == password; // also need to hash the password in production
        }
    }
}