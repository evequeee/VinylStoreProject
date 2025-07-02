using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Exceptions;
using ProjectVinylStore.Business.Services;
using ProjectVinylStore.Filters;
using System.Security.Claims;

namespace ProjectVinylStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        /// Process checkout and create order
        /// <returns>Order confirmation details</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderConfirmationDto>>> ProcessCheckout([FromBody] CheckoutRequestDto checkoutRequest)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(currentUserId))
            {
                throw ApiException.Unauthorized("User not authenticated");
            }

            // Ensure the checkout is for the current user
            checkoutRequest.UserId = currentUserId;

            // Validate checkout request
            if (checkoutRequest.Items == null || !checkoutRequest.Items.Any())
            {
                throw ApiException.BadRequest("No items in the order", "EMPTY_ORDER");
            }

            try
            {
                var result = await _checkoutService.ProcessCheckoutAsync(checkoutRequest);
                return Ok(ApiResponse<OrderConfirmationDto>.SuccessResult(result, "Order processed successfully"));
            }
            catch (InvalidOperationException ex)
            {
                throw ApiException.BadRequest(ex.Message, "CHECKOUT_FAILED");
            }
        }

        /// Validate payment details
        /// <returns>Payment validation result</returns>
        [HttpPost("validate-payment")]
        public async Task<ActionResult<ApiResponse<bool>>> ValidatePayment([FromBody] PaymentDetailsDto paymentDetails)
        {
            var isValid = await _checkoutService.ValidatePaymentAsync(paymentDetails);
            
            var message = isValid ? "Payment details are valid" : "Payment details are invalid";
            return Ok(ApiResponse<bool>.SuccessResult(isValid, message));
        }

        /// Calculate total amount for items
        /// <returns>Total amount calculation</returns>
        [HttpPost("calculate-total")]
        public async Task<ActionResult<ApiResponse<decimal>>> CalculateTotal([FromBody] List<OrderItemDto> items)
        {
            if (items == null || !items.Any())
            {
                throw ApiException.BadRequest("No items provided for calculation", "NO_ITEMS");
            }

            var total = await _checkoutService.CalculateTotalAsync(items);
            return Ok(ApiResponse<decimal>.SuccessResult(total, "Total calculated successfully"));
        }
    }
}