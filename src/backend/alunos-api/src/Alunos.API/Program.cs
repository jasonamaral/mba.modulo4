using Alunos.API.Extensions;
using Alunos.Application.Interfaces.Services;
using Alunos.Application.Services;
using Alunos.Domain.Interfaces;
using Alunos.Infrastructure.Data;
using Alunos.Infrastructure.Repositories;
using Alunos.Infrastructure.Services;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configurar Mapster
TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);

// Configurar Entity Framework - Configuração condicional baseada no ambiente
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    // SQLite para desenvolvimento
    builder.Services.AddDbContext<AlunoDbContext>(options =>
        options.UseSqlite(connectionString ?? "Data Source=../../../../data/alunos-dev.db"));
}
else
{
    // SQL Server para produção
    builder.Services.AddDbContext<AlunoDbContext>(options =>
        options.UseSqlServer(connectionString ?? "Server=localhost;Database=AlunosDB;Trusted_Connection=true;TrustServerCertificate=true;"));
}

builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();

builder.Services.AddScoped<IAlunoAppService, AlunoAppService>();

builder.Services.AddSwaggerConfiguration();

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
            ClockSkew = TimeSpan.Zero
        };

        // Adicionar eventos para debug
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Falha na autenticação: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado com sucesso");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge: {context.Error}, {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<RegistrarUsuarioEventHandler>();

builder.Services.AddHostedService<RegistrarUsuarioEventConsumer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwaggerConfiguration();

// Remover HTTPS redirection
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Inicializar banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.Run();