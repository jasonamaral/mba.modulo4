using BFF.Application.Interfaces.Services;
using BFF.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Notification;

namespace BFF.API.Controllers;

/// <summary>
/// Controller para endpoints de dashboard
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : BffController
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger, INotificador notificador)
        : base(notificador)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém o dashboard do aluno
    /// </summary>
    /// <returns>Dados agregados do dashboard do aluno</returns>
    [HttpGet("aluno")]
    [Authorize(Roles = "Aluno")]
    public async Task<IActionResult> GetDashboardAluno()
    {
        try
        {
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
            {
                return ProcessarErro(System.Net.HttpStatusCode.Unauthorized, "Token inválido");
            }

            var dashboard = await _dashboardService.GetDashboardAlunoAsync(userId);
            return RespostaPadraoApi(System.Net.HttpStatusCode.OK, dashboard, "Dashboard do aluno obtido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição de dashboard do aluno");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém o dashboard do administrador
    /// </summary>
    /// <returns>Dados agregados do dashboard do administrador</returns>
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDashboardAdmin()
    {
        try
        {
            var dashboard = await _dashboardService.GetDashboardAdminAsync();
            return RespostaPadraoApi(System.Net.HttpStatusCode.OK, dashboard, "Dashboard do administrador obtido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição de dashboard do admin");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }
} 