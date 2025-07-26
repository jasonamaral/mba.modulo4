using BFF.API.Extensions;
using BFF.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurações
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// HttpClient para comunicação com outras APIs
builder.Services.AddHttpClient();

// Services
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
