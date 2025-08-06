namespace BFF.Domain.DTOs;

/// <summary>
/// Resultado de uma ação de API com status e dados
/// </summary>
/// <typeparam name="T">Tipo dos dados</typeparam>
public class ApiActionResult<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Código de status HTTP
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Mensagem de sucesso ou erro
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Dados da resposta (quando Success = true)
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Conteúdo do erro (quando Success = false)
    /// </summary>
    public object? ErrorContent { get; set; }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static ApiActionResult<T> SuccessResult(T data, string message = "Operação realizada com sucesso")
    {
        return new ApiActionResult<T>
        {
            Success = true,
            StatusCode = 200,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Cria um resultado de erro
    /// </summary>
    public static ApiActionResult<T> ErrorResult(int statusCode, string message, object? errorContent = null)
    {
        return new ApiActionResult<T>
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
            ErrorContent = errorContent
        };
    }
} 