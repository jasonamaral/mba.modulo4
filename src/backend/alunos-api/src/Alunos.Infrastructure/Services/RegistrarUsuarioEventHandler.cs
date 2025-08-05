using Alunos.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Alunos.Infrastructure.Services;

public class RegistrarUsuarioEventHandler(IAlunoRepository alunoRepository, ILogger<RegistrarUsuarioEventHandler> logger)
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly ILogger<RegistrarUsuarioEventHandler> _logger = logger;

    public async Task HandleAsync(RegistrarUsuarioEvent evento)
    {
        //try
        //{
        //    var userIdGuid = evento.CodigoUsuarioAutenticacao;
        //    var alunoExistente = await _alunoRepository.GetByCodigoUsuarioAsync(userIdGuid);
        //    if (alunoExistente != null)
        //    {
        //        return;
        //    }

        //    // Criar novo perfil de aluno
        //    var novoAluno = new Aluno(
        //        codigoUsuarioAutenticacao: userIdGuid,
        //        nome: evento.Nome,
        //        email: evento.Email,
        //        cpf: evento.CPF,
        //        dataNascimento: evento.DataNascimento,
        //        telefone: evento.Telefone,
        //        genero: evento.Genero,
        //        cidade: evento.Cidade,
        //        estado: evento.Estado,
        //        cep: evento.CEP
        //    );

        //    // Salvar no repositório
        //    await _alunoRepository.AdicionarAsync(novoAluno);
        //    await _alunoRepository.UnitOfWork.Commit();

        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Erro ao processar evento UserRegistered para usuário: {UserId}", evento.UserId);
        //    throw;
        //}
    }
}