using BFF.API.Configuration;
using BFF.API.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddApiConfiguration();

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
    }
}
