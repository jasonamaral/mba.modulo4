using BFF.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace BFF.Infrastructure.Services;

public abstract class BaseApiService
{
    protected readonly IApiClientService _apiClient;
    protected readonly ILogger _logger;

    protected BaseApiService(IApiClientService apiClient, ILogger logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    protected void ConfigureAuthToken(string token)
    {
        _apiClient.ClearDefaultHeaders();
        _apiClient.AddDefaultHeader("Authorization", $"Bearer {token}");
        _apiClient.AddDefaultHeader("Accept", "application/json");
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
}