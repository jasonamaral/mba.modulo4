using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.CadastrarAula
{
    public class CadastrarAulaCommandHandler : IRequestHandler<CadastrarAulaCommand, CommandResult>
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private Guid _raizAgregacao;

        public CadastrarAulaCommandHandler(IAulaRepository aulaRepository,
                                            ICursoRepository cursoRepository,
                                            IMediatorHandler mediatorHandler)
        {
            _aulaRepository = aulaRepository;
            _cursoRepository = cursoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CommandResult> Handle(CadastrarAulaCommand request, CancellationToken cancellationToken)
        {
            _raizAgregacao = request.RaizAgregacao;
            if (!await ValidarRequisicao(request))
                return request.Resultado;

            var aula = new Aula(
                request.CursoId,
                request.Nome,
                request.Descricao,
                request.Numero,
                request.DuracaoMinutos,
                request.VideoUrl,
                request.TipoAula,
                request.IsObrigatoria,
                request.Observacoes);

            await _aulaRepository.CadastrarAulaAsync(aula);

            if (await _aulaRepository.UnitOfWork.Commit())
                request.Resultado.Data = aula.Id;

            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(CadastrarAulaCommand request)
        {
            request.DefinirValidacao(new CadastrarAulaCommandValidator().Validate(request));
            if (!request.EhValido())
            {
                foreach (var erro in request.Erros)
                {
                    await _mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), erro));
                }
                return false;
            }

            var curso = await _cursoRepository.ObterPorIdAsync(request.CursoId);
            if (curso == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Curso não encontrado"));
                return false;
            }

            if (await _aulaRepository.ExistePorNumeroAsync(request.CursoId, request.Numero))
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Já existe uma aula com este número no curso"));
                return false;
            }

            return true;
        }
    }
}
