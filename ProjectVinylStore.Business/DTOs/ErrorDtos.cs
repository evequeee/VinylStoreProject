using System.Text.Json.Serialization;

namespace ProjectVinylStore.Business.DTOs
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? TraceId { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StackTrace { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ErrorResponse? Error { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ApiResponse<T> SuccessResult(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> ErrorResult(ErrorResponse error)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Error = error
            };
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse SuccessResult(string message = "")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message
            };
        }

        public static new ApiResponse ErrorResult(ErrorResponse error)
        {
            return new ApiResponse
            {
                Success = false,
                Error = error
            };
        }
    }
}