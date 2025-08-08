using BFF.API.Configuration;
using BFF.API.Extensions;
using BFF.API.Handlers;
using BFF.Domain.Settings;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration();

// HttpClient para comunicação com outras APIs com Polly
var resilienceSettings = builder.Configuration.GetSection("ResilienceSettings").Get<ResilienceSettings>();
builder.Services.AddTransient<AuthorizationDelegatingHandler>();
builder.Services.AddHttpClient("ApiClient")
    .AddHttpMessageHandler<AuthorizationDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy(resilienceSettings))
    .AddPolicyHandler(GetCircuitBreakerPolicy(resilienceSettings));

// Configuração global do JSON
builder.Services.AddJsonConfiguration();

builder.Services.AddAuthorization();

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
