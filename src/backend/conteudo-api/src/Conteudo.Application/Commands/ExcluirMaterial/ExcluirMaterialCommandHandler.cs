using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.ExcluirMaterial
{
    public class ExcluirMaterialCommandHandler : IRequestHandler<ExcluirMaterialCommand, CommandResult>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ExcluirMaterialCommandHandler(IMaterialRepository materialRepository,
                                             IMediatorHandler mediatorHandler)
        {
            _materialRepository = materialRepository;
            _mediatorHandler = mediatorHandler;
        }
        public async Task<CommandResult> Handle(ExcluirMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.ObterPorIdAsync(request.Id);

            if (!await ValidarRequisicao(request, material))
                return request.Resultado;

            await _materialRepository.ExcluirMaterialAsync(material.Id);

            if (!await _materialRepository.UnitOfWork.Commit())
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Material), "Erro ao excluir o material."));
                return request.Resultado;
            }

            request.Resultado.Data = true;
            return request.Resultado;
        }

        private async Task<bool> ValidarRequisicao(ExcluirMaterialCommand request, Material material)
        {
            request.DefinirValidacao(new ExcluirMaterialCommandValidator().Validate(request));

            if (!request.EhValido())
            {
                foreach (var erro in request.Erros)
                {
                    await _mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Material), erro));
                }
                return false;
            }

            if (material == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(request.RaizAgregacao, nameof(Material), "Material não encontrado."));
                return false;
            }

            return true;
        }
    }
}
