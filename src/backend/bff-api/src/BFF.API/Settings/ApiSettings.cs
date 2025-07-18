namespace BFF.API.Settings;

public class ApiSettings
{
    public string AuthApiUrl { get; set; } = string.Empty;
    public string ConteudoApiUrl { get; set; } = string.Empty;
    public string AlunosApiUrl { get; set; } = string.Empty;
    public string PagamentosApiUrl { get; set; } = string.Empty;
}

public class RedisSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int Database { get; set; } = 0;
    public string KeyPrefix { get; set; } = "bff:";
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
}

public class ResilienceSettings
{
    public int RetryCount { get; set; } = 3;
    public int CircuitBreakerThreshold { get; set; } = 3;
    public TimeSpan CircuitBreakerDuration { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan TimeoutDuration { get; set; } = TimeSpan.FromSeconds(30);
}

public class CacheSettings
{
    public TimeSpan DashboardExpiration { get; set; } = TimeSpan.FromMinutes(10);
    public TimeSpan UserProfileExpiration { get; set; } = TimeSpan.FromMinutes(60);
    public TimeSpan CursoDetailsExpiration { get; set; } = TimeSpan.FromMinutes(15);
    public TimeSpan RelatoriosExpiration { get; set; } = TimeSpan.FromMinutes(5);
} 