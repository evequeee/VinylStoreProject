namespace ProjectVinylStore.Business.DTOs
{
    public class OrderHistoryDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Completed"; // For future status implementation
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Completed";
        public UserDto User { get; set; } = new UserDto();
    }

    public class UserOrderHistoryDto
    {
        public UserDto User { get; set; } = new UserDto();
        public List<OrderHistoryDto> Orders { get; set; } = new List<OrderHistoryDto>();
        public int TotalOrders => Orders.Count;
        public decimal TotalSpent => Orders.Sum(o => o.TotalAmount);
    }
}