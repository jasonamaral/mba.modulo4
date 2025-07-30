using Conteudo.API.Configuration;
using Conteudo.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health Check
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", DateTime = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

// Migration Helper
app.UseDbMigrationHelper();
app.Run();
