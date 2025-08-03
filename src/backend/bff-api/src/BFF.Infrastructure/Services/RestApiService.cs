using BFF.Application.Interfaces.Services;
using BFF.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace BFF.Infrastructure.Services;

public class RestApiService : IRestApiService, IDisposable
{
    private readonly RestClient _restClient;
    private readonly ResilienceSettings _resilienceSettings;
    private readonly ILogger<RestApiService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private string? _baseAddress;

    public RestApiService(
        IHttpClientFactory httpClientFactory,
        IOptions<ResilienceSettings> resilienceOptions,
        ILogger<RestApiService> logger)
    {
        var httpClient = httpClientFactory.CreateClient("ApiClient");
        _restClient = new RestClient(httpClient);
        _resilienceSettings = resilienceOptions.Value;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public void SetBaseAddress(string baseAddress)
    {
        if (Uri.TryCreate(baseAddress, UriKind.Absolute, out var uri))
        {
            _baseAddress = baseAddress;
        }
        else
        {
            _logger.LogError("Endereço base inválido: {BaseAddress}", baseAddress);
            throw new ArgumentException("Endereço base inválido", nameof(baseAddress));
        }
    }

    public void AddDefaultHeader(string key, string value)
    {
        _restClient.AddDefaultHeader(key, value);
    }

    public async Task<T?> GetAsync<T>(string endpoint, IDictionary<string, string>? headers = null) where T : class
    {
        try
        {
            var request = CreateRequest(endpoint, Method.Get, headers);
            var response = await ExecuteRequestAsync(request, endpoint);

            if (response?.IsSuccessful == true && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<T>(response.Content, _jsonOptions);
                return result;
            }

            LogFailedRequest("GET", endpoint, response);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição GET para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, IDictionary<string, string>? headers = null) where TRequest : class where TResponse : class
    {
        try
        {
            var restRequest = CreateRequest(endpoint, Method.Post, headers).AddJsonBody(request);

            var response = await ExecuteRequestAsync(restRequest, endpoint);

            if (response?.IsSuccessful == true && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<TResponse>(response.Content, _jsonOptions);
                return result;
            }

            LogFailedRequest("POST", endpoint, response);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição POST para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request, IDictionary<string, string>? headers = null) where TRequest : class where TResponse : class
    {
        try
        {
            var restRequest = CreateRequest(endpoint, Method.Put, headers).AddJsonBody(request);

            var response = await ExecuteRequestAsync(restRequest, endpoint);

            if (response?.IsSuccessful == true && !string.IsNullOrEmpty(response.Content))
            {
                var result = JsonSerializer.Deserialize<TResponse>(response.Content, _jsonOptions);
                return result;
            }

            LogFailedRequest("PUT", endpoint, response);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição PUT para {Endpoint}", endpoint);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint, IDictionary<string, string>? headers = null)
    {
        try
        {
            var request = CreateRequest(endpoint, Method.Delete, headers);
            var response = await ExecuteRequestAsync(request, endpoint);

            if (response?.IsSuccessful == true)
            {
                return true;
            }

            LogFailedRequest("DELETE", endpoint, response);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na requisição DELETE para {Endpoint}", endpoint);
            return false;
        }
    }

    private RestRequest CreateRequest(string endpoint, Method method, IDictionary<string, string>? headers = null)
    {
        var fullUrl = !string.IsNullOrEmpty(_baseAddress) ? $"{_baseAddress.TrimEnd('/')}/{endpoint.TrimStart('/')}" : endpoint;

        var request = new RestRequest(fullUrl, method);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        return request;
    }

    private async Task<RestResponse?> ExecuteRequestAsync(RestRequest request, string endpoint)
    {
        try
        {
            return await _restClient.ExecuteAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar requisição para {Endpoint}", endpoint);
            return null;
        }
    }

    private void LogFailedRequest(string method, string endpoint, RestResponse? response)
    {
        if (response != null)
        {
            _logger.LogWarning("Requisição {Method} falhou para {Endpoint}. Status: {StatusCode}, Mensagem: {ErrorMessage}",
                method, endpoint, response.StatusCode, response.ErrorMessage);
        }
        else
        {
            _logger.LogWarning("Requisição {Method} falhou para {Endpoint}. Resposta nula", method, endpoint);
        }
    }

    public void Dispose()
    {
        _restClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}