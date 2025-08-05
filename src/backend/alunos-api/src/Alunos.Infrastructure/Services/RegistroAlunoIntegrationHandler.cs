using Alunos.Application.Commands;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alunos.Infrastructure.Services;

public class RegistroAlunoIntegrationHandler : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RegistroAlunoIntegrationHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMessageBus _bus;
    private volatile bool _isReady = false;

    public RegistroAlunoIntegrationHandler(
        IServiceProvider serviceProvider,
        ILogger<RegistroAlunoIntegrationHandler> logger,
        IConfiguration configuration,
        IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void SetResponder()
    {
        _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request => await RegistrarAluno(request));

        _bus.AdvancedBus.Connected += OnConnect;
    }

    private void OnConnect(object? s, EventArgs e)
    {
        SetResponder();
    }

    private async Task<ResponseMessage> RegistrarAluno(UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(
            message.Id,
            message.Nome,
            message.Email,
            message.Cpf,
            message.DataNascimento,
            message.Telefone,
            message.Genero,
            message.Cidade,
            message.Estado,
            message.Cep,
            message.Foto,
            message.EhAdministrador,
            message.DataCadastro
        );

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

        var sucesso = await mediator.EnviarComando(clienteCommand);

        return new ResponseMessage(sucesso);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parando consumidor de eventos UserRegistered");
        _isReady = false;
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}