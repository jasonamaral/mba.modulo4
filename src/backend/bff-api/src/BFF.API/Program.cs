using BFF.API.Extensions;
using BFF.API.Settings;
using BFF.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configurações
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));
builder.Services.Configure<ResilienceSettings>(builder.Configuration.GetSection("ResilienceSettings"));

// Redis Cache
var redisSettings = builder.Configuration.GetSection("RedisSettings").Get<RedisSettings>();
if (redisSettings != null && !string.IsNullOrEmpty(redisSettings.ConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisSettings.ConnectionString;
        options.InstanceName = redisSettings.KeyPrefix;
    });
}
else
{
    // Fallback para cache em memória se Redis não estiver disponível
    builder.Services.AddMemoryCache();
}

// HttpClient para comunicação com outras APIs com Polly
var resilienceSettings = builder.Configuration.GetSection("ResilienceSettings").Get<ResilienceSettings>();
builder.Services.AddHttpClient("ApiClient")
    .AddPolicyHandler(GetRetryPolicy(resilienceSettings))
    .AddPolicyHandler(GetCircuitBreakerPolicy(resilienceSettings));

// Services
builder.Services.AddScoped<BFF.Application.Interfaces.Services.ICacheService, BFF.Infrastructure.Services.CacheService>();
builder.Services.AddScoped<BFF.Application.Interfaces.Services.IHttpClientService, BFF.Infrastructure.Services.HttpClientService>();
builder.Services.AddScoped<BFF.Application.Interfaces.Services.IDashboardService, BFF.Infrastructure.Services.DashboardService>();

// Configuração global do JSON
builder.Services.AddJsonConfiguration();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerConfiguration();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings != null)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });
}

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        var corsSettings = builder.Configuration.GetSection("CORS");
        var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>();
        var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>();
        var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>();

        policy.WithOrigins(allowedOrigins ?? new[] { "http://localhost:4200" })
              .WithMethods(allowedMethods ?? new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" })
              .WithHeaders(allowedHeaders ?? new[] { "Content-Type", "Authorization" })
              .AllowCredentials();
    });
});

// Health Checks
builder.Services.AddHealthChecks();

// Configurar para usar a porta especificada
var urls = builder.Configuration["Urls"];
if (!string.IsNullOrEmpty(urls))
{
    builder.WebHost.UseUrls(urls);
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

// Remover HTTPS redirection
// app.UseHttpsRedirection();

app.UseCors("AllowedOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

// Métodos auxiliares para políticas de resiliência
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ResilienceSettings? settings)
{
    var retryCount = settings?.RetryCount ?? 3;
    var timeout = settings?.TimeoutDuration ?? TimeSpan.FromSeconds(30);
    
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                Console.WriteLine($"Tentativa {retryCount} em {timespan}s");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ResilienceSettings? settings)
{
    var threshold = settings?.CircuitBreakerThreshold ?? 3;
    var duration = settings?.CircuitBreakerDuration ?? TimeSpan.FromSeconds(30);
    
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            threshold,
            duration,
            onBreak: (exception, duration) =>
            {
                Console.WriteLine($"Circuit breaker aberto por {duration}s");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit breaker fechado");
            });
}
