using Alunos.Application.Interfaces.Repositories;
using Alunos.Domain.Entities;
using Alunos.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Alunos.Application.EventHandlers;

/// <summary>
/// Handler para processar evento de usuário registrado
/// </summary>
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

    /// <summary>
    /// Processa o evento de usuário registrado criando o perfil do aluno
    /// </summary>
    /// <param name="evento">Evento com dados do usuário</param>
    public async Task HandleAsync(UserRegisteredEvent evento)
    {
        try
        {
            _logger.LogInformation("Processando evento UserRegistered para usuário: {UserId}", evento.UserId);

            // Verificar se já existe um aluno com este código de usuário
            var alunoExistente = await _alunoRepository.GetByCodigoUsuarioAsync(evento.UserId);
            if (alunoExistente != null)
            {
                _logger.LogWarning("Aluno já existe para o usuário: {UserId}", evento.UserId);
                return;
            }

            // Criar novo perfil de aluno
            var novoAluno = new Aluno(
                codigoUsuarioAutenticacao: evento.UserId,
                nome: evento.Nome,
                email: evento.Email,
                dataNascimento: evento.DataNascimento
            );

            // Salvar no repositório
            await _alunoRepository.AddAsync(novoAluno);
            await _alunoRepository.SaveChangesAsync();

            _logger.LogInformation("Perfil de aluno criado com sucesso para usuário: {UserId}, AlunoId: {AlunoId}", 
                evento.UserId, novoAluno.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento UserRegistered para usuário: {UserId}", evento.UserId);
            throw;
        }
    }
} 