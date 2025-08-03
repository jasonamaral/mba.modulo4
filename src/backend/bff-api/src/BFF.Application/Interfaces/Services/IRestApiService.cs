namespace BFF.Application.Interfaces.Services;

public interface IRestApiService
{
    Task<T?> GetAsync<T>(string endpoint, IDictionary<string, string>? headers = null) where T : class;

    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, IDictionary<string, string>? headers = null) where TRequest : class where TResponse : class;

    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request, IDictionary<string, string>? headers = null) where TRequest : class where TResponse : class;

    Task<bool> DeleteAsync(string endpoint, IDictionary<string, string>? headers = null);

    void SetBaseAddress(string baseAddress);

    void AddDefaultHeader(string key, string value);
}