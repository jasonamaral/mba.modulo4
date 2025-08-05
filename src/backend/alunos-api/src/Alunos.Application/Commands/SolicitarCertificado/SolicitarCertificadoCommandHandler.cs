using MediatR;
using Core.Mediator;
using Alunos.Domain.Interfaces;
using Core.Communication;
using Core.Messages;

namespace Alunos.Application.Commands.SolicitarCertificado;
public class SolicitarCertificadoCommandHandler(IAlunoRepository alunoRepository, 
    IMediatorHandler mediatorHandler) : IRequestHandler<SolicitarCertificadoCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(SolicitarCertificadoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!ValidarRequisicao(request)) { return request.CommandResult; }
        if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.CommandResult; }

        aluno.RequisitarCertificadoConclusao(request.MatriculaCursoId, request.NotaFinal, request.PathCertificado, request.NomeInstrutor);
        var certificado = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId).Certificado;

        await _alunoRepository.AdicionarCertificadoMatriculaCursoAsync(certificado);
        await _alunoRepository.UnitOfWork.Commit();
        return request.CommandResult;
    }

    private bool ValidarRequisicao(SolicitarCertificadoCommand request)
    {
        request.DefinirValidacao(new SolicitarCertificadoCommandValidator().Validate(request));

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
