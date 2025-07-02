using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Services;
using ProjectVinylStore.DataAccess.Entities;
using ProjectVinylStore.DataAccess.Interfaces;

namespace ProjectVinylStore.Business.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserOrderHistoryDto> GetUserOrderHistoryAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetUserWithOrdersAsync(userId);
            if (user == null)
            {
                return new UserOrderHistoryDto();
            }

            var orderDtos = user.Orders.Select(o => new OrderHistoryDto
            {
                Id = o.Id,
                ProductName = o.ProductName,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount
            }).OrderByDescending(o => o.OrderDate).ToList();

            return new UserOrderHistoryDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email ?? string.Empty
                },
                Orders = orderDtos
            };
        }

        public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return null;

            var user = await _unitOfWork.Users.GetByIdAsync(order.UserId) as ApplicationUser;

            return new OrderDetailDto
            {
                Id = order.Id,
                ProductName = order.ProductName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                User = new UserDto
                {
                    Id = user?.Id ?? string.Empty,
                    Name = user != null ? $"{user.FirstName} {user.LastName}".Trim() : string.Empty,
                    Email = user?.Email ?? string.Empty
                }
            };
        }

        public async Task<IEnumerable<OrderHistoryDto>> GetRecentOrdersAsync(int count = 10)
        {
            var orders = await _unitOfWork.Orders.GetRecentOrdersAsync(count);
            return orders.Select(o => new OrderHistoryDto
            {
                Id = o.Id,
                ProductName = o.ProductName,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount
            });
        }
    }
}