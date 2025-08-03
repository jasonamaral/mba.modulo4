using MediatR;
using Alunos.Domain.Interfaces;
using Core.Mediator;
using Core.Communication;
using Core.SharedDtos.Conteudo;
using Alunos.Domain.Entities;
using Alunos.Domain.ValueObjects;
using Core.Messages;
using Core.Mediator.AlunoCommands;
using Core.Mediator.AlunoEvents;

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
            if (!ValidarRequisicao(request)) { return request.CommandResult; }
            if (!ObterAluno(request.AlunoId, out Domain.Entities.Aluno aluno)) { return request.CommandResult; }
            if (!ObterAulaCurso(request.CursoDto, request.AulaId, aluno, out AulaDto aulaDto)) { return request.CommandResult; }

            // Capturo o histórico anterior (se existir)
            // Isto é um "bug" do EF que não consegue identificar corretamente o estado de mudança do objeto
            MatriculaCurso matriculaCurso = aluno.ObterMatriculaCursoPeloId(request.MatriculaCursoId);
            HistoricoAprendizado historicoAntigo = aluno.ObterHistoricoAprendizado(request.MatriculaCursoId, request.AulaId);

            // Registro o histórico
            aluno.RegistrarHistoricoAprendizado(request.MatriculaCursoId, request.AulaId, aulaDto.Descricao, (byte)(aulaDto.DuracaoMinutos), request.DataTermino);

            // Capturo o novo histórico
            HistoricoAprendizado historicoAtual = aluno.ObterHistoricoAprendizado(request.MatriculaCursoId, request.AulaId);

            await _alunoRepository.AtualizarEstadoHistoricoAprendizadoAsync(historicoAntigo, historicoAtual);
            await _alunoRepository.UnitOfWork.Commit();
            return request.CommandResult;
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

    private bool ObterAulaCurso(CursoDto cursoDto, Guid aulaId, Domain.Entities.Aluno aluno, out AulaDto aulaDto)
    {
        //aulaDto = new();
        //MatriculaCurso matriculaCurso = aluno.ObterMatriculaCursoPeloId(matriculaCursoId);
        //CursoDto cursoDto = _cursoService.ObterPorIdAsync(matriculaCurso.CursoId).Result;
        aulaDto = cursoDto?.Aulas?.FirstOrDefault(x => x.Id == aulaId) ?? new();

        ////if (cursoDto == null)
        ////{
        ////    _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Matrícula do curso deste aluno não encontrado")).GetAwaiter().GetResult();
        ////    return false;
        ////}

        ////if (cursoDto?.Aulas == null)
        ////{
        ////    _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Curso informado não possui nenhuma aula cadastrada")).GetAwaiter().GetResult();
        ////    return false;
        ////}

        ////if (aulaDto == null) 
        ////{ 
        ////    _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aula deste curso não encontrado")).GetAwaiter().GetResult();
        ////    return false;
        ////}

        //if (!aulaDto.Ativo)
        //{
        //    _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Domain.Entities.Aluno), "Aula informada está inativa")).GetAwaiter().GetResult();
        //    return false;
        //}

        return true;
    }
}
