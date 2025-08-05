using Alunos.Application.Commands;
using Alunos.Application.Interfaces.Repositories;
using Alunos.Domain.Entities;
using Core.Communication;
using Core.Messages;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Alunos.Application.Commands;

public class RegistrarClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, CommandResult>
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
            var alunoExistente = await _alunoRepository.GetByCodigoUsuarioAsync(request.Id);
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
                dataNascimento: request.DataNascimento,
                telefone: request.Telefone,
                genero: request.Genero,
                cidade: request.Cidade,
                estado: request.Estado,
                cep: request.Cep
            );

            // Salvar no repositório
            await _alunoRepository.AddAsync(novoAluno);
            await _alunoRepository.SaveChangesAsync();

            return new CommandResult(request.ValidationResult);
        }
        catch (Exception ex)
        {
            request.ValidationResult.Errors.Add(new ValidationFailure("Exception", $"Erro ao registrar aluno: {ex.Message}"));
            return new CommandResult(request.ValidationResult);
        }
    }
} 