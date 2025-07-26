namespace Auth.API.Models.Responses;

/// <summary>
/// Modelo de erro da API
/// </summary>
public class ApiError
{
    /// <summary>
    /// Mensagem de erro
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Detalhes do erro
    /// </summary>
    public IEnumerable<string>? Details { get; set; }

    /// <summary>
    /// Código do erro
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Timestamp do erro
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 