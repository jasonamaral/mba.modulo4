using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Communication;
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
            return request.Resultado;

        try
        {
            // Verificar se já existe um aluno com este código de usuário
            var alunoExistente = await _alunoRepository.ObterPorCodigoUsuarioAsync(request.Id);
            if (alunoExistente != null)
            {
                request.Validacao.Errors.Add(new ValidationFailure("Id", "Já existe um aluno cadastrado com este código de usuário."));
                return request.Resultado;
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

            return request.Resultado;
        }
        catch (Exception ex)
        {
            request.Validacao.Errors.Add(new ValidationFailure("Exception", $"Erro ao registrar aluno: {ex.Message}"));
            return request.Resultado;
        }
    }
}