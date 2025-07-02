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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderHistoryService _orderHistoryService;

        public OrdersController(IOrderHistoryService orderHistoryService)
        {
            _orderHistoryService = orderHistoryService;
        }

        /// Get current user's order history
        /// <returns>User's order history with total statistics</returns>
        [HttpGet("my-orders")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserOrderHistoryDto>>> GetMyOrderHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                throw ApiException.Unauthorized("User not authenticated");
            }

            var result = await _orderHistoryService.GetUserOrderHistoryAsync(userId);
            return Ok(ApiResponse<UserOrderHistoryDto>.SuccessResult(result, "Order history retrieved successfully"));
        }

        /// Get user order history by user ID (Admin only)
        /// <returns>User's order history</returns>
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<UserOrderHistoryDto>>> GetUserOrderHistory(string userId)
        {
            var result = await _orderHistoryService.GetUserOrderHistoryAsync(userId);
            return Ok(ApiResponse<UserOrderHistoryDto>.SuccessResult(result, "User order history retrieved successfully"));
        }

        /// Get order details by order ID
        /// <returns>Detailed order information</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<OrderDetailDto>>> GetOrder(int id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            var order = await _orderHistoryService.GetOrderDetailAsync(id);
            
            if (order == null)
            {
                throw ApiException.NotFound("Order", id);
            }

            // Check if user can access this order (own order or admin)
            if (!isAdmin && order.User.Id != currentUserId)
            {
                throw ApiException.Forbidden("You can only access your own orders");
            }

            return Ok(ApiResponse<OrderDetailDto>.SuccessResult(order, "Order details retrieved successfully"));
        }

        /// Get recent orders (Admin only)
        /// <returns>List of recent orders</returns>
        [HttpGet("recent")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderHistoryDto>>>> GetRecentOrders([FromQuery] int count = 10)
        {
            if (count <= 0 || count > 100)
            {
                throw ApiException.BadRequest("Count must be between 1 and 100", "INVALID_COUNT");
            }

            var result = await _orderHistoryService.GetRecentOrdersAsync(count);
            return Ok(ApiResponse<IEnumerable<OrderHistoryDto>>.SuccessResult(result, "Recent orders retrieved successfully"));
        }
    }
}