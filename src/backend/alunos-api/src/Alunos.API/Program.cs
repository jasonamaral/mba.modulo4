using Alunos.API.Configurations;
using Alunos.API.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddApiConfiguration();
        builder.Services.AddMessageBusConfiguration(builder.Configuration);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }

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
    }
}