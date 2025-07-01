using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Services
{
    public interface ICheckoutService
    {
        // Checkout & Payment
        Task<OrderConfirmationDto> ProcessCheckoutAsync(CheckoutRequestDto checkoutRequest);
        Task<bool> ValidatePaymentAsync(PaymentDetailsDto paymentDetails);
        Task<decimal> CalculateTotalAsync(List<OrderItemDto> items);
    }
}