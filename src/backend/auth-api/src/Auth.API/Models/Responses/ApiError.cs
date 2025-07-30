namespace Auth.API.Models.Responses;

public class ApiError
{
    public string Message { get; set; } = string.Empty;

    public IEnumerable<string>? Details { get; set; }

    public string? ErrorCode { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}