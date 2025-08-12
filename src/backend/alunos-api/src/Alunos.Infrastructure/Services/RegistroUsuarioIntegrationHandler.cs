using Alunos.Domain.Interfaces;
using Core.Messages;
using Core.Messages.Integration;
using MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alunos.Infrastructure.Services;


public class RegistroUsuarioIntegrationHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageBus _bus;
    private readonly ILogger<RegistroUsuarioIntegrationHandler> _logger;

    public RegistroUsuarioIntegrationHandler(
        IServiceProvider serviceProvider, 
        IMessageBus bus,
        ILogger<RegistroUsuarioIntegrationHandler> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry simples para aguardar RabbitMQ ficar disponível dentro do container
        var tentativas = 0;
        while (tentativas < 10 && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                SetResponder();
                _logger.LogInformation("Responder de RegistroUsuario configurado com sucesso");
                break;
            }
            catch (Exception ex)
            {
                tentativas++;
                _logger.LogWarning(ex, "Falha ao configurar responder (tentativa {Tentativa}). Aguardando RabbitMQ...", tentativas);
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }

    private void SetResponder()
    {
        _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request => await ProcessarUsuarioRegistrado(request));
        _bus.AdvancedBus.Connected += OnConnect;
    }

    private void OnConnect(object? s, EventArgs e)
    {
        SetResponder();
    }


    private async Task<ResponseMessage> ProcessarUsuarioRegistrado(UsuarioRegistradoIntegrationEvent message)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var integrationService = scope.ServiceProvider.GetRequiredService<IRegistroUsuarioIntegrationService>();

            return await integrationService.ProcessarUsuarioRegistradoAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento de usuário registrado. ID: {UserId}", message.Id);
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Exception", ex.Message));
            return new ResponseMessage(validationResult);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parando consumidor de eventos UserRegistered");
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}