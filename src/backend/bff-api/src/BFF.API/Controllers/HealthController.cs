using Microsoft.AspNetCore.Mvc;
using Core.Notification;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace BFF.API.Controllers;

/// <summary>
/// Controller para verificação de saúde da API
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : BffController
{
    public HealthController(IMediatorHandler mediator,
                          INotificationHandler<DomainNotificacaoRaiz> notifications,
                          INotificador notificador) : base(mediator, notifications, notificador)
    {
    }
    /// <summary>
    /// Verifica se a API está funcionando
    /// </summary>
    /// <returns>Status da API</returns>
    [HttpGet]
    public IActionResult Get()
    {
        var healthData = new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            service = "BFF API"
        };
        
        return RespostaPadraoApi<object>(System.Net.HttpStatusCode.OK, healthData, "API funcionando normalmente");
    }

    /// <summary>
    /// Endpoint para verificação de status com informações detalhadas
    /// </summary>
    /// <returns>Informações detalhadas sobre a API</returns>
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        var statusData = new
        {
            api = "BFF API",
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            timestamp = DateTime.UtcNow,
            uptime = DateTime.UtcNow,
            status = "running",
            description = "Backend for Frontend API - Orquestração dos Microsserviços da Plataforma Educacional"
        };
        
        return RespostaPadraoApi<object>(System.Net.HttpStatusCode.OK, statusData, "Status da API obtido com sucesso");
    }
} 