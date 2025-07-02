using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Exceptions;
using ProjectVinylStore.Business.Services;
using ProjectVinylStore.DataAccess.Interfaces;
using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.Business.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderConfirmationDto> ProcessCheckoutAsync(CheckoutRequestDto checkoutRequest)
        {
            // Validate payment
            var isPaymentValid = await ValidatePaymentAsync(checkoutRequest.PaymentDetails);
            if (!isPaymentValid)
            {
                throw ApiException.BadRequest("Payment validation failed", "PAYMENT_INVALID");
            }

            // Calculate total
            var totalAmount = await CalculateTotalAsync(checkoutRequest.Items);

            if (totalAmount <= 0)
            {
                throw ApiException.BadRequest("Order total must be greater than zero", "INVALID_TOTAL");
            }

            // Create order
            var order = new Order
            {
                UserId = checkoutRequest.UserId,
                ProductName = string.Join(", ", checkoutRequest.Items.Select(i => i.ProductName)),
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            return new OrderConfirmationDto
            {
                OrderId = order.Id,
                OrderNumber = $"ORD-{order.Id:D6}",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = "Confirmed",
                Items = checkoutRequest.Items,
                ShippingDetails = checkoutRequest.ShippingDetails
            };
        }

        public Task<bool> ValidatePaymentAsync(PaymentDetailsDto paymentDetails)
        {
            // Basic validation - in production implement proper payment gateway validation
            if (string.IsNullOrWhiteSpace(paymentDetails.PaymentMethod))
                return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(paymentDetails.CardNumber))
                return Task.FromResult(false);

            // Additional validations can be added here
            return Task.FromResult(true);
        }

        public async Task<decimal> CalculateTotalAsync(List<OrderItemDto> items)
        {
            decimal total = 0;

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                {
                    throw ApiException.BadRequest($"Invalid quantity for item {item.ProductName}", "INVALID_QUANTITY");
                }

                // Verify vinyl exists and get current price
                var vinyl = await _unitOfWork.VinylRecords.GetByIdAsync(item.VinylRecordId);
                if (vinyl == null)
                {
                    throw ApiException.NotFound("Vinyl record", item.VinylRecordId);
                }

                if (vinyl.StockQuantity < item.Quantity)
                {
                    throw ApiException.BadRequest($"Not enough stock for {vinyl.Title}. Available: {vinyl.StockQuantity}", "INSUFFICIENT_STOCK");
                }

                total += vinyl.Price * item.Quantity;
            }

            return total;
        }
    }
}