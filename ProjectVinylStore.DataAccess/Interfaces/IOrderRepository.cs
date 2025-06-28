using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
        Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
    }
}