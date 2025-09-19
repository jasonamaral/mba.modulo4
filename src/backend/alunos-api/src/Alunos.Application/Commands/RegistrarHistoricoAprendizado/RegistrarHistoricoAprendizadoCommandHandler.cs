using Alunos.Application.Events.RegistrarProblemaHistorico;
using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Alunos.Domain.ValueObjects;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Commands.RegistrarHistoricoAprendizado;

public class RegistrarHistoricoAprendizadoCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<RegistrarHistoricoAprendizadoCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(RegistrarHistoricoAprendizadoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _raizAgregacao = request.RaizAgregacao;
            if (!ValidarRequisicao(request)) { return request.Resultado; }
            if (!ObterAluno(request.AlunoId, out Aluno aluno)) { return request.Resultado; }

            MatriculaCurso matriculaCurso = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId);
            HistoricoAprendizado historicoAntigo = aluno.ObterHistoricoAprendizado(request.MatriculaCursoId, request.AulaId);

            aluno.RegistrarHistoricoAprendizado(request.MatriculaCursoId, request.AulaId, request.NomeAula, request.DuracaoMinutos, request.DataTermino);

            HistoricoAprendizado historicoAtual = aluno.ObterHistoricoAprendizado(request.MatriculaCursoId, request.AulaId);

            await _alunoRepository.AtualizarEstadoHistoricoAprendizadoAsync(historicoAntigo, historicoAtual);
            if (await _alunoRepository.UnitOfWork.Commit()) { request.Resultado.Data = true; }

            return request.Resultado;
        }
        catch (Exception ex)
        {
            string mensagem = $"Erro registrando histórico de Aprendizado. Exception: {ex}";
            await _mediatorHandler.PublicarEvento(new RegistrarProblemaHistoricoAprendizadoEvent(request.AlunoId,
                request.MatriculaCursoId,
                request.AulaId,
                request.DataTermino,
                mensagem));
            throw;
        }
    }

    private bool ValidarRequisicao(RegistrarHistoricoAprendizadoCommand request)
    {
        request.DefinirValidacao(new RegistrarHistoricoAprendizadoCommandValidator().Validate(request));

        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), erro)).GetAwaiter().GetResult();
            }

            return false;
        }

        return true;
    }

    private bool ObterAluno(Guid alunoId, out Domain.Entities.Aluno aluno)
    {
        aluno = _alunoRepository.ObterPorIdAsync(alunoId).Result;
        if (aluno == null)
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aluno não encontrado.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }
}
