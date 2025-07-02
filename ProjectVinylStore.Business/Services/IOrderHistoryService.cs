using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Services
{
    public interface IOrderHistoryService
    {
        // User Order History
        Task<UserOrderHistoryDto> GetUserOrderHistoryAsync(string userId);
        Task<OrderDetailDto?> GetOrderDetailAsync(int orderId);
        Task<IEnumerable<OrderHistoryDto>> GetRecentOrdersAsync(int count = 10);
    }
}