using ProjectVinylStore.Business.DTOs;
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
                throw new InvalidOperationException("Payment validation failed");
            }

            // Calculate total
            var totalAmount = await CalculateTotalAsync(checkoutRequest.Items);

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

        public async Task<bool> ValidatePaymentAsync(PaymentDetailsDto paymentDetails)
        {
            // todo: Implement actual payment validation
            return !string.IsNullOrEmpty(paymentDetails.PaymentMethod) &&
                   !string.IsNullOrEmpty(paymentDetails.CardNumber);
        }

        public async Task<decimal> CalculateTotalAsync(List<OrderItemDto> items)
        {
            decimal total = 0;

            foreach (var item in items)
            {
                // Verify vinyl exists and get current price
                var vinyl = await _unitOfWork.VinylRecords.GetByIdAsync(item.VinylRecordId);
                if (vinyl != null)
                {
                    total += vinyl.Price * item.Quantity;
                }
            }

            return total;
        }
    }
}