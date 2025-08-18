using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirAula
{
    public class ExcluirAulaCommandHandler : IRequestHandler<ExcluirAulaCommand, CommandResult>
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ExcluirAulaCommandHandler(IAulaRepository aulaRepository,
                                         IMediatorHandler mediatorHandler)
        {
            _aulaRepository = aulaRepository;
            _mediatorHandler = mediatorHandler;
        }
        public async Task<CommandResult> Handle(ExcluirAulaCommand request, CancellationToken cancellationToken)
        {
            var aula = await _aulaRepository.ObterPorIdAsync(request.Id);

            if (!await ValidarRequisicao(request, aula))
                return request.Resultado;

            await _aulaRepository.ExcluirAulaAsync(aula.Id);

            if (!await _aulaRepository.UnitOfWork.Commit())
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Aula), "Erro ao excluir aula."));
                return request.Resultado;
            }

            request.Resultado.Data = true;
            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(ExcluirAulaCommand request, Aula aula)
        {
            request.DefinirValidacao(new ExcluirAulaCommandValidator().Validate(request));

            if (!request.EhValido())
            {
                foreach (var erro in request.Erros)
                {
                    await _mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Aula), erro));
                }
                return false;
            }

            if (aula == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Aula), "Aula não encontrada."));
                return false;
            }

            return true;
        }
    }
}
