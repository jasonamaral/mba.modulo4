using Alunos.API.Configurations;
using Alunos.Infrastructure.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddApiConfiguration();
        builder.Services.AddMessageBusConfiguration(builder.Configuration);

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

        // Inicializar banco de dados
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AlunoDbContext>();

            InitializeDatabaseAsync(context);
        }

        // Migration Helper
        app.Run();
    }

    static void InitializeDatabaseAsync(AlunoDbContext context)
    {
        // Criar banco se n�o existir
        context.Database.EnsureCreatedAsync();
    }
}