using Alunos.API.Extensions;
using Alunos.Application.EventHandlers;
using Alunos.Application.Interfaces.Repositories;
using Alunos.Application.Interfaces.Services;
using Alunos.Application.Services;
using Alunos.Infrastructure.Data;
using Alunos.Infrastructure.Repositories;
using Alunos.Infrastructure.Services;
using Core.Identidade;
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
    builder.Services.AddDbContext<AlunosDbContext>(options =>
        options.UseSqlite(connectionString ?? "Data Source=../../../../data/alunos-dev.db"));
}
else
{
    // SQL Server para produção
    builder.Services.AddDbContext<AlunosDbContext>(options =>
        options.UseSqlServer(connectionString ?? "Server=localhost;Database=AlunosDB;Trusted_Connection=true;TrustServerCertificate=true;"));
}

builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();

builder.Services.AddScoped<IAlunoAppService, AlunoAppService>();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddScoped<UserRegisteredEventHandler>();

builder.Services.AddHostedService<UserRegisteredEventConsumer>();

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
    var context = scope.ServiceProvider.GetRequiredService<AlunosDbContext>();
    await context.Database.EnsureCreatedAsync();
}

app.Run();