using Alunos.Application.Commands;
using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Alunos.Application.Commands;

public class RegistrarClienteCommandHandler : IRequestHandler<RegistrarClienteCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository;

    public RegistrarClienteCommandHandler(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<CommandResult> Handle(RegistrarClienteCommand request, CancellationToken cancellationToken)
    {
        if (!request.EhValido())
            return new CommandResult(request.ValidationResult);

        try
        {
            // Verificar se já existe um aluno com este código de usuário
            var alunoExistente = await _alunoRepository.ObterPorCodigoUsuarioAsync(request.Id);
            if (alunoExistente != null)
            {
                request.ValidationResult.Errors.Add(new ValidationFailure("Id", "Já existe um aluno cadastrado com este código de usuário."));
                return new CommandResult(request.ValidationResult);
            }

            // Criar novo perfil de aluno
            var novoAluno = new Aluno(
                codigoUsuarioAutenticacao: request.Id,
                nome: request.Nome,
                email: request.Email,
                cpf: request.Cpf,
                dataNascimento: request.DataNascimento
            );

            // Salvar no repositório
            await _alunoRepository.AdicionarAsync(novoAluno);
            await _alunoRepository.UnitOfWork.Commit();

            return new CommandResult(request.ValidationResult);
        }
        catch (Exception ex)
        {
            request.ValidationResult.Errors.Add(new ValidationFailure("Exception", $"Erro ao registrar aluno: {ex.Message}"));
            return new CommandResult(request.ValidationResult);
        }
    }
} 