using Alunos.Application.Interfaces.Repositories;
using Alunos.Domain.Entities;
using Alunos.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Alunos.Application.EventHandlers;

public class UserRegisteredEventHandler
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly ILogger<UserRegisteredEventHandler> _logger;

    public UserRegisteredEventHandler(
        IAlunoRepository alunoRepository,
        ILogger<UserRegisteredEventHandler> logger)
    {
        _alunoRepository = alunoRepository;
        _logger = logger;
    }

    public async Task HandleAsync(UserRegisteredEvent evento)
    {
        try
        {

            // Verificar se já existe um aluno com este código de usuário
            var userIdGuid = Guid.Parse(evento.UserId);
            var alunoExistente = await _alunoRepository.GetByCodigoUsuarioAsync(userIdGuid);
            if (alunoExistente != null)
            {
                return;
            }

            // Criar novo perfil de aluno
            var novoAluno = new Aluno(
                codigoUsuarioAutenticacao: userIdGuid,
                nome: evento.Nome,
                email: evento.Email,
                cpf: evento.CPF,
                dataNascimento: evento.DataNascimento,
                telefone: evento.Telefone,
                genero: evento.Genero,
                cidade: evento.Cidade,
                estado: evento.Estado,
                cep: evento.CEP
            );

            // Salvar no repositório
            await _alunoRepository.AddAsync(novoAluno);
            await _alunoRepository.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento UserRegistered para usuário: {UserId}", evento.UserId);
            throw;
        }
    }
}