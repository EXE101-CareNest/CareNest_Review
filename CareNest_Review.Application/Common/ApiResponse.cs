namespace CareNest_Review.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool success, string? message = null, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        // Shortcut static methods
        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
            => new(true, message, data);

        public static ApiResponse<T> Failure(string message)
            => new(false, message);
    }

    // Dùng để map JSON của external API (status/code/message/meta/data)
    public class ExternalApiEnvelope<T>
    {
        public string? Status { get; set; }
        public int? Code { get; set; }
        public string? Message { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? RequestId { get; set; }
        public string? Version { get; set; }
        public object? Meta { get; set; }
        public T? Data { get; set; }
    }
}
