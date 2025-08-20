using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarAula
{
    public class AtualizarAulaCommandHandler : IRequestHandler<AtualizarAulaCommand, CommandResult>
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private Guid _raizAgregacao;

        public AtualizarAulaCommandHandler(IAulaRepository aulaRepository,
                                           ICursoRepository cursoRepository,
                                           IMediatorHandler mediatorHandler)
        {
            _aulaRepository = aulaRepository;
            _cursoRepository = cursoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CommandResult> Handle(AtualizarAulaCommand request, CancellationToken cancellationToken)
        {
            _raizAgregacao = request.RaizAgregacao;
            var aulaExistente = await _aulaRepository.ObterPorIdAsync(request.Id, false);

            if (!await ValidarRequisicao(request, aulaExistente))
                return request.Resultado;

            aulaExistente.AtualizarInformacoes(
                request.Nome,
                request.Descricao,
                request.Numero,
                request.DuracaoMinutos,
                request.VideoUrl,
                request.TipoAula,
                request.IsObrigatoria,
                request.Observacoes);

            await _aulaRepository.AtualizarAulaAsync(aulaExistente);

            if (await _aulaRepository.UnitOfWork.Commit())
            {
                request.Resultado.Data = true;
            }

            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(AtualizarAulaCommand request, Aula aula)
        {
            request.DefinirValidacao(new AtualizarAulaCommandValidator().Validate(request));
            if (!request.EhValido())
            {
                foreach (var erro in request.Erros)
                {
                    await _mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), erro));
                }
                return false;
            }

            if (aula == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Aula não encontrada"));
                return false;
            }

            var curso = await _cursoRepository.ObterPorIdAsync(request.CursoId);
            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Curso não encontrado"));
                return false;
            }

            if (aula.CursoId != request.CursoId)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Aula não pertence ao curso informado"));
                return false;
            }

            if (await _aulaRepository.ExistePorNumeroAsync(aula.CursoId, request.Numero, request.Id))
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Já existe uma aula com este número no curso"));
                return false;
            }

            return true;
        }
    }
}
