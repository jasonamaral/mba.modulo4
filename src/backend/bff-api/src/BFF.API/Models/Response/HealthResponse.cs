namespace BFF.API.Models.Response;

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public List<ServiceHealthResponse> Services { get; set; } = new();
}

public class ServiceHealthResponse
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ResponseTime { get; set; } = string.Empty;
    public DateTime LastCheck { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ApiStatusResponse
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public TimeSpan Uptime { get; set; }
    public string Status { get; set; } = string.Empty;
    public Dictionary<string, object> Configuration { get; set; } = new();
}