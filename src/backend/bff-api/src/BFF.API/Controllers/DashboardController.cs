using BFF.Application.Interfaces.Services;
using BFF.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.API.Controllers;

/// <summary>
/// Controller para endpoints de dashboard
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
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
                return Unauthorized("Token inválido");
            }

            var dashboard = await _dashboardService.GetDashboardAlunoAsync(userId);
            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição de dashboard do aluno");
            return StatusCode(500, new { error = "Erro interno do servidor" });
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
            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição de dashboard do admin");
            return StatusCode(500, new { error = "Erro interno do servidor" });
        }
    }
} 