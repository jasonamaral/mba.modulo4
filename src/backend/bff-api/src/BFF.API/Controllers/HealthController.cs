using Microsoft.AspNetCore.Mvc;

namespace BFF.API.Controllers;

/// <summary>
/// Controller para verificação de saúde da API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Verifica se a API está funcionando
    /// </summary>
    /// <returns>Status da API</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            service = "BFF API"
        });
    }

    /// <summary>
    /// Endpoint para verificação de status com informações detalhadas
    /// </summary>
    /// <returns>Informações detalhadas sobre a API</returns>
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            api = "BFF API",
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            timestamp = DateTime.UtcNow,
            uptime = DateTime.UtcNow,
            status = "running",
            description = "Backend for Frontend API - Orquestração dos Microsserviços da Plataforma Educacional"
        });
    }
} 