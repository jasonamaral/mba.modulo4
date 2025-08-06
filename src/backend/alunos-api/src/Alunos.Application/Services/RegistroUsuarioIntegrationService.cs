using Alunos.Application.Commands;
using Alunos.Domain.Interfaces;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Microsoft.Extensions.Logging;

namespace Alunos.Application.Services;

public class RegistroUsuarioIntegrationService : IRegistroUsuarioIntegrationService
{
    private readonly IMediatorHandler _mediatorHandler;
    private readonly ILogger<RegistroUsuarioIntegrationService> _logger;

    public RegistroUsuarioIntegrationService(
        IMediatorHandler mediatorHandler,
        ILogger<RegistroUsuarioIntegrationService> logger)
    {
        _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResponseMessage> ProcessarUsuarioRegistradoAsync(UsuarioRegistradoIntegrationEvent message)
    {
        try
        {

            var registrarClienteCommand = new RegistrarClienteCommand(
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

            var resultado = await _mediatorHandler.EnviarComando(registrarClienteCommand);

            if (resultado.IsValid)
            {
                return new ResponseMessage(resultado);
            }
            else
            {
                _logger.LogWarning("Falha na validação do comando de registro. ID: {UserId}, Erros: {Erros}", message.Id, string.Join(", ", resultado.Errors.Select(e => e.ErrorMessage)));
                return new ResponseMessage(resultado);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento de usuário registrado. ID: {UserId}", message.Id);
            var validationResult = new FluentValidation.Results.ValidationResult();
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("Exception", ex.Message));
            return new ResponseMessage(validationResult);
        }
    }
}