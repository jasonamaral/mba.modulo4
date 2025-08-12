using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using Core.SharedDtos.Conteudo;
using MediatR;

namespace Alunos.Application.Commands.ConcluirCurso;
public class ConcluirCursoCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<ConcluirCursoCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(ConcluirCursoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.Resultado; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.Resultado; }
        var matriculaCurso = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId);

        if (!ValidarSeMatriculaCursoPodeSerConcluido(aluno, request.CursoDto)) { return request.Resultado; }

        aluno.ConcluirCurso(request.MatriculaCursoId);

        await _alunoRepository.AtualizarAsync(aluno);
        if (await _alunoRepository.UnitOfWork.Commit()) { request.Resultado.Data = true; }

        return request.Resultado;
    }

    private bool ValidarRequisicao(ConcluirCursoCommand request)
    {
        request.DefinirValidacao(new ConcluirCursoCommandValidator().Validate(request));
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

    private bool ValidarSeMatriculaCursoPodeSerConcluido(Aluno aluno, CursoDto cursoDto)
    {
        bool retorno = true;
        if (aluno.ObterQuantidadeAulasPendenteMatriculaCurso(cursoDto.Id) > 0)
        {
            retorno = false;
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Existem aulas pendentes para este curso")).GetAwaiter().GetResult();
        }

        int totalAulasAtivos = cursoDto.Aulas.Count();
        int totalAulasMatricula = aluno.ObterQuantidadeAulasMatriculaCurso(cursoDto.Id);
        if (totalAulasMatricula < totalAulasAtivos)
        {
            retorno = false;
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Curso não pode ser concluído. Aulas pendentes.")).GetAwaiter().GetResult();
        }

        return retorno;
    }
}
