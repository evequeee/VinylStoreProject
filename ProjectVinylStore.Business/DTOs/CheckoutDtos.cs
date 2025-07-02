namespace ProjectVinylStore.Business.DTOs
{
    public class CheckoutRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public ShippingDetailsDto ShippingDetails { get; set; } = new ShippingDetailsDto();
        public PaymentDetailsDto PaymentDetails { get; set; } = new PaymentDetailsDto();
    }

    public class OrderItemDto
    {
        public int VinylRecordId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal => Quantity * Price;
    }

    public class ShippingDetailsDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class PaymentDetailsDto
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
    }

    public class OrderConfirmationDto
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public ShippingDetailsDto ShippingDetails { get; set; } = new ShippingDetailsDto();
    }
}