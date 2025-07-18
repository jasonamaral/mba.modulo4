using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pagamentos.API.Extensions;
using Pagamentos.Application.Interfaces;
using Pagamentos.Infrastructure.Data;
using Pagamentos.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração do Entity Framework
builder.Services.AddDbContext<PagamentosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Registro dos repositórios
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
builder.Services.AddScoped<IReembolsoRepository, ReembolsoRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<IWebhookRepository, WebhookRepository>();

// TODO: Adicionar demais serviços
// builder.Services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
// builder.Services.AddScoped<IWebhookService, WebhookService>();
// builder.Services.AddMediatR(typeof(ProcessarPagamentoCommandHandler));

// Configuração do Swagger
builder.Services.AddSwaggerConfiguration();

// Configuração do Hangfire (TODO: Implementar)
// builder.Services.AddHangfire(configuration => configuration
//     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
//     .UseSimpleAssemblyNameTypeSerializer()
//     .UseRecommendedSerializerSettings()
//     .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddHangfireServer();

// Configuração do HttpClient para integração com outras APIs
builder.Services.AddHttpClient("AlunosAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:AlunosApiUrl"]);
});

builder.Services.AddHttpClient("ConteudoAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ConteudoApiUrl"]);
});

builder.Services.AddHttpClient("AuthAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:AuthApiUrl"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfiguration();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// TODO: Configurar Hangfire Dashboard
// if (app.Environment.IsDevelopment())
// {
//     app.UseHangfireDashboard("/hangfire", new DashboardOptions
//     {
//         Authorization = new[] { new HangfireAuthorizationFilter() }
//     });
// }

app.MapControllers();

// Aplicar migrações automaticamente em desenvolvimento
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PagamentosDbContext>();
    
    try
    {
        context.Database.EnsureCreated();
        // TODO: Implementar seed de dados se necessário
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrações do banco de dados");
    }
}

app.Run();
