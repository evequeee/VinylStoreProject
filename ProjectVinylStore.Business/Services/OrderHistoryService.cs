using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Services;
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

        public async Task<UserOrderHistoryDto> GetUserOrderHistoryAsync(int userId)
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
                    Name = user.Name,
                    Email = user.Email
                },
                Orders = orderDtos
            };
        }

        public async Task<OrderDetailDto?> GetOrderDetailAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return null;

            var user = await _unitOfWork.Users.GetByIdAsync(order.UserId);

            return new OrderDetailDto
            {
                Id = order.Id,
                ProductName = order.ProductName,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
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