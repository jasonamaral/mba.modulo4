using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Conteudo.Application.Commands.PublicarAula
{
    public class PublicarAulaCommandHandler : IRequestHandler<PublicarAulaCommand, CommandResult>
    {
        private readonly IAulaRepository _aulaRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private Guid _raizAgregacao;

        public PublicarAulaCommandHandler(IAulaRepository aulaRepository, IMediatorHandler mediatorHandler)
        {
            _aulaRepository = aulaRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CommandResult> Handle(PublicarAulaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _raizAgregacao = request.RaizAgregacao;

                if (!await ValidarRequisicao(request))
                    return request.Resultado;

                await _aulaRepository.PublicarAulaAsync(request.Id);

                if (await _aulaRepository.UnitOfWork.Commit())
                {
                    request.Resultado.Data = request.Id;
                }

                return request.Resultado;
            }
            catch (Exception ex)
            {
                request.Validacao.Errors.Add(new ValidationFailure("Exception", $"Erro ao publicar aula: {ex.Message}"));
                return request.Resultado;
            }
        }

        private async Task<bool> ValidarRequisicao(PublicarAulaCommand request)
        {
            var aula = await _aulaRepository.ObterPorIdAsync(request.Id);
            if (aula == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aula), "Aula não encontrada"));
                return false;
            }

            return true;
        }
    }
}
