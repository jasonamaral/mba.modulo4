using BFF.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace BFF.Infrastructure.Services;

public abstract class BaseApiService
{
    protected readonly IRestApiService _restApiService;
    protected readonly ILogger _logger;

    protected BaseApiService(IRestApiService restApiService, ILogger logger)
    {
        _restApiService = restApiService;
        _logger = logger;
    }

    protected void AddAuthHeader(string token)
    {
        _restApiService.AddDefaultHeader("Authorization", $"Bearer {token}");
        _restApiService.AddDefaultHeader("Accept", "application/json");
    }

    protected static IDictionary<string, string> CreateAuthHeaders(string token)
    {
        return new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {token}" },
            { "Accept", "application/json" },
            { "Content-Type", "application/json" }
        };
    }

    protected async Task<T?> ExecuteWithErrorHandling<T>(Func<Task<T?>> operation, string operationName, params object[] parameters)
    {
        try
        {
            var result = await operation();

            if (result == null)
            {
                _logger.LogWarning("{OperationName} retornou null. Parâmetros: {@Parameters}", operationName, parameters);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar {OperationName}. Parâmetros: {@Parameters}", operationName, parameters);
            return default;
        }
    }

    protected bool ValidateToken(string token, string operationName)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning("Token inválido para operação {OperationName}", operationName);
            return false;
        }

        return true;
    }

    protected bool ValidateId(Guid id, string operationName)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning("ID inválido para operação {OperationName}", operationName);
            return false;
        }

        return true;
    }
}