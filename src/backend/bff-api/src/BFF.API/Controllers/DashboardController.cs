using BFF.API.Extensions;
using BFF.API.Models.Response;
using BFF.API.Services;
using BFF.Application.Interfaces.Services;
using BFF.Domain.DTOs;
using Core.Mediator;
using Core.Messages;
using Core.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.API.Controllers;

/// <summary>
/// Controller de Dashboard no BFF - Agrega dados de múltiplas APIs
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : BffController
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService dashboardService,
        ILogger<DashboardController> logger,
        IMediatorHandler mediator,
        INotificationHandler<DomainNotificacaoRaiz> notifications,
        INotificador notificador) : base(mediator, notifications, notificador)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Obter dashboard do aluno
    /// </summary>
    /// <returns>Dados do dashboard do aluno</returns>
    [HttpGet("aluno")]
    [Authorize(Roles = "Usuario")]
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

            if (dashboard != null)
            {
                return RespostaPadraoApi<DashboardAlunoDto>(System.Net.HttpStatusCode.OK, dashboard, "Dashboard do aluno obtido com sucesso");
            }

            return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Dashboard não encontrado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dashboard do aluno");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obter dashboard do administrador
    /// </summary>
    /// <returns>Dados do dashboard do administrador</returns>
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDashboardAdmin()
    {
        try
        {
            var dashboard = await _dashboardService.GetDashboardAdminAsync();

            if (dashboard != null)
            {
                return RespostaPadraoApi<DashboardAdminDto>(System.Net.HttpStatusCode.OK, dashboard, "Dashboard do administrador obtido com sucesso");
            }

            return ProcessarErro(System.Net.HttpStatusCode.NotFound, "Dashboard não encontrado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dashboard do administrador");
            return ProcessarErro(System.Net.HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }
} 