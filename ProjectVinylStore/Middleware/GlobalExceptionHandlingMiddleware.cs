using Microsoft.AspNetCore.Mvc;
using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Exceptions;
using System.Net;
using System.Text.Json;

namespace ProjectVinylStore.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorResponse = CreateErrorResponse(exception, context);
            context.Response.StatusCode = errorResponse.StatusCode;

            var apiResponse = ApiResponse.ErrorResult(errorResponse);
            var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private ErrorResponse CreateErrorResponse(Exception exception, HttpContext context)
        {
            var traceId = context.TraceIdentifier;

            return exception switch
            {
                ApiException apiEx => new ErrorResponse
                {
                    Message = apiEx.Message,
                    ErrorCode = apiEx.ErrorCode,
                    StatusCode = apiEx.StatusCode,
                    TraceId = traceId,
                    ValidationErrors = apiEx.ValidationErrors,
                    StackTrace = _environment.IsDevelopment() ? apiEx.StackTrace : null
                },

                TaskCanceledException => new ErrorResponse
                {
                    Message = "The request was cancelled.",
                    ErrorCode = "REQUEST_CANCELLED",
                    StatusCode = 408,
                    TraceId = traceId
                },

                TimeoutException => new ErrorResponse
                {
                    Message = "The request timed out.",
                    ErrorCode = "REQUEST_TIMEOUT",
                    StatusCode = 408,
                    TraceId = traceId
                },

                UnauthorizedAccessException => new ErrorResponse
                {
                    Message = "Unauthorized access.",
                    ErrorCode = "UNAUTHORIZED",
                    StatusCode = 401,
                    TraceId = traceId
                },

                ArgumentException argEx => new ErrorResponse
                {
                    Message = argEx.Message,
                    ErrorCode = "INVALID_ARGUMENT",
                    StatusCode = 400,
                    TraceId = traceId,
                    StackTrace = _environment.IsDevelopment() ? argEx.StackTrace : null
                },

                InvalidOperationException invalidOpEx => new ErrorResponse
                {
                    Message = invalidOpEx.Message,
                    ErrorCode = "INVALID_OPERATION",
                    StatusCode = 400,
                    TraceId = traceId,
                    StackTrace = _environment.IsDevelopment() ? invalidOpEx.StackTrace : null
                },

                _ => new ErrorResponse
                {
                    Message = _environment.IsDevelopment() ? exception.Message : "An internal server error occurred.",
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    StatusCode = 500,
                    TraceId = traceId,
                    StackTrace = _environment.IsDevelopment() ? exception.StackTrace : null
                }
            };
        }
    }
}