namespace BFF.Application.Interfaces.Services;

public interface IHttpClientService
{
    Task<T?> GetAsync<T>(string endpoint) where T : class;
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request) 
        where TRequest : class 
        where TResponse : class;
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request) 
        where TRequest : class 
        where TResponse : class;
    Task<bool> DeleteAsync(string endpoint);
} 