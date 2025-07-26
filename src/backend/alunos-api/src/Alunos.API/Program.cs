using Alunos.API.Extensions;
using Alunos.Application.EventHandlers;
using Alunos.Application.Interfaces.Repositories;
using Alunos.Application.Interfaces.Services;
using Alunos.Application.Services;
using Alunos.Infrastructure.Data;
using Alunos.Infrastructure.Repositories;
using Alunos.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar Entity Framework
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=alunos.db";

builder.Services.AddDbContext<AlunosDbContext>(options =>
    options.UseSqlite(connectionString));

// Configurar Repositórios
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();

// Configurar Application Services
builder.Services.AddScoped<IAlunoAppService, AlunoAppService>();

// Configurar Swagger
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
    });

builder.Services.AddAuthorization();

// Configurar Event Handlers
builder.Services.AddScoped<UserRegisteredEventHandler>();

// Configurar Background Services
builder.Services.AddHostedService<UserRegisteredEventConsumer>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar Health Check
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerConfiguration();

// Remover HTTPS redirection
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configurar Health Check
app.MapHealthChecks("/health");

// Inicializar banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AlunosDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.Run();
