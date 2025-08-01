using BFF.Application.Interfaces.Services;
using BFF.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace BFF.Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly ResilienceSettings _resilienceSettings;
    private readonly ILogger<HttpClientService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpClientService(
        IHttpClientFactory httpClientFactory,
        IOptions<ResilienceSettings> resilienceOptions,
        ILogger<HttpClientService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _resilienceSettings = resilienceOptions.Value;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<T?> GetAsync<T>(string endpoint) where T : class
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }

            _logger.LogWarning("Requisição GET falhou para {Endpoint}. Status: {StatusCode}", endpoint, response.StatusCode);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição GET para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
            }

            _logger.LogWarning("Requisição POST falhou para {Endpoint}. Status: {StatusCode}", endpoint, response.StatusCode);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição POST para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
            }

            _logger.LogWarning("Requisição PUT falhou para {Endpoint}. Status: {StatusCode}", endpoint, response.StatusCode);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição PUT para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            _logger.LogWarning("Requisição DELETE falhou para {Endpoint}. Status: {StatusCode}", endpoint, response.StatusCode);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição DELETE para {Endpoint}", endpoint);
            return false;
        }
    }
}