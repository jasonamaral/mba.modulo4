using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Alunos.Infrastructure.Services;

public class RegistrarUsuarioEventHandler(IAlunoRepository alunoRepository, ILogger<RegistrarUsuarioEventHandler> logger)
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly ILogger<RegistrarUsuarioEventHandler> _logger = logger;

    public async Task HandleAsync(RegistrarUsuarioEvent evento)
    {
        try
        {
            var userIdGuid = evento.CodigoUsuarioAutenticacao;
            var alunoExistente = await _alunoRepository.ObterPorIdAsync(userIdGuid);
            if (alunoExistente != null)
            {
                return;
            }

            // Criar novo perfil de aluno
            var novoAluno = new Aluno(userIdGuid, evento.Nome, evento.Email, evento.Cpf, evento.DataNascimento);

            // Salvar no repositório
            await _alunoRepository.AdicionarAsync(novoAluno);
            await _alunoRepository.UnitOfWork.Commit();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar evento UserRegistered para usuário: {UserId}", evento.CodigoUsuarioAutenticacao);
            throw;
        }
    }
}