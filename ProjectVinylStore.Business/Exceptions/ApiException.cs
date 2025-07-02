using System;
using System.Collections.Generic;

namespace ProjectVinylStore.Business.Exceptions
{
    public class ApiException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }
        public Dictionary<string, string[]>? ValidationErrors { get; }

        public ApiException(string message, int statusCode = 500, string errorCode = "INTERNAL_ERROR")
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public ApiException(string message, Exception innerException, int statusCode = 500, string errorCode = "INTERNAL_ERROR")
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }

        public ApiException(string message, Dictionary<string, string[]> validationErrors, int statusCode = 400, string errorCode = "VALIDATION_ERROR")
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
            ValidationErrors = validationErrors;
        }

        public static ApiException NotFound(string resource, object? key = null)
        {
            var message = key != null
                ? $"{resource} with key '{key}' was not found."
                : $"{resource} was not found.";
            return new ApiException(message, 404, "NOT_FOUND");
        }

        public static ApiException BadRequest(string message, string errorCode = "BAD_REQUEST")
        {
            return new ApiException(message, 400, errorCode);
        }

        public static ApiException Unauthorized(string message = "Unauthorized access.")
        {
            return new ApiException(message, 401, "UNAUTHORIZED");
        }

        public static ApiException Forbidden(string message = "Access forbidden.")
        {
            return new ApiException(message, 403, "FORBIDDEN");
        }

        public static ApiException Validation(Dictionary<string, string[]> validationErrors)
        {
            return new ApiException("One or more validation errors occurred.", validationErrors);
        }

        public static ApiException Validation(string message, string errorCode = "VALIDATION_ERROR")
        {
            return new ApiException(message, 400, errorCode);
        }

        public static ApiException InternalError(string message = "An internal server error occurred.")
        {
            return new ApiException(message, 500, "INTERNAL_ERROR");
        }
    }
}