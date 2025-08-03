using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Mediator;
using Core.Mediator.AlunoCommands;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Commands.MatricularAluno;
public class MatricularAlunoCommandHandler(IAlunoRepository alunoRepository,
    IMediatorHandler mediatorHandler) : IRequestHandler<MatricularAlunoCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public IAlunoRepository AlunoRepository => _alunoRepository;

    public async Task<CommandResult> Handle(MatricularAlunoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.CommandResult; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.CommandResult; }

        aluno.MatricularAlunoEmCurso(request.CursoId, request.NomeCurso, request.ValorCurso, request.Observacao);
        var matricula = aluno.ObterMatriculaPorCursoId(request.CursoId);
        await AlunoRepository.AdicionarMatriculaCursoAsync(matricula);
        await AlunoRepository.UnitOfWork.Commit();

        // Ver aqui a necessidade de gerar o link de pagamento
        //await _mediatorHandler.PublicarEvento(new GerarLinkPagamentoEvent(matricula.Id, request.AlunoId, request.CursoId, request.ValorCurso));

        return request.CommandResult;
    }

    private bool ValidarRequisicao(MatricularAlunoCommand request)
    {
        request.DefinirValidacao(new MatricularAlunoCommandValidator().Validate(request));
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
        aluno = AlunoRepository.ObterPorIdAsync(alunoId).Result;
        if (aluno == null)
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aluno não encontrado.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }
}
