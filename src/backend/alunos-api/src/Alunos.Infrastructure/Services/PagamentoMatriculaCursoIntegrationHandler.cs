using Alunos.Domain.Interfaces;
using Core.Messages;
using Core.Messages.Integration;
using MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alunos.Infrastructure.Services;

public class PagamentoMatriculaCursoIntegrationHandler(IServiceProvider serviceProvider,
    IMessageBus bus,
    ILogger<PagamentoMatriculaCursoIntegrationHandler> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IMessageBus _bus = bus;
    private readonly ILogger<PagamentoMatriculaCursoIntegrationHandler> _logger = logger;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<PagamentoMatriculaCursoIntegrationEvent, ResponseMessage>(async request => await ProcessarPagamentoMatriculaCurso(request));
        _bus.AdvancedBus.Connected += OnConnect;
    }

    private void OnConnect(object? s, EventArgs e)
    {
        SetResponder();
    }

    private async Task<ResponseMessage> ProcessarPagamentoMatriculaCurso(PagamentoMatriculaCursoIntegrationEvent message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var integrationService = scope.ServiceProvider.GetRequiredService<IRegistroPagamentoIntegrationService>();

            return await integrationService.ProcessarPagamentoMatriculaCursoAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento de pagamento de matrícula-curso. Id Aluno: {AlunoId} Id Curso: {CursoId}", message.AlunoId, message.CursoId);
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Exception", ex.Message));
            return new ResponseMessage(validationResult);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parando consumidor de eventos PagamentoMatriculaCursoIntegrationHandler");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
