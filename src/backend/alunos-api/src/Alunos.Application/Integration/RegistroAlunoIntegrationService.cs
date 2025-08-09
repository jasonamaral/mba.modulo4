using Alunos.Application.Commands;
using Alunos.Application.Commands.CadastrarAluno;
using Alunos.Domain.Interfaces;
using Core.Mediator;
using Core.Messages;
using Core.Messages.Integration;
using Microsoft.Extensions.Logging;

namespace Alunos.Application.Integration;
public class RegistroAlunoIntegrationService(IMediatorHandler mediatorHandler,
    ILogger<RegistroAlunoIntegrationService> logger) : IRegistroAlunoIntegrationService
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private readonly ILogger<RegistroAlunoIntegrationService> _logger = logger;

    public async Task<ResponseMessage> ProcessarAlunoRegistradoAsync(AlunoRegistradoIntegrationEvent message)
    {
        try
        {
            var registrarClienteCommand = new CadastrarAlunoCommand(message.Id,
                message.Nome,
                message.Email,
                message.Cpf,
                message.DataNascimento,
                message.Telefone,
                message.Genero,
                message.Cidade,
                message.Estado,
                message.Cep,
                message.Foto
                //message.EhAdministrador,
                //message.DataCadastro
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