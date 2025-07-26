namespace BFF.API.Settings;

public class RedisSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int Database { get; set; } = 0;
    public string KeyPrefix { get; set; } = "bff:";
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);
}
