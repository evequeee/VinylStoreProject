using System;

namespace ProjectVinylStore.DataAccess.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
