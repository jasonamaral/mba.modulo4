using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarMaterial
{
    public class AtualizarMaterialCommandHandler : IRequestHandler<AtualizarMaterialCommand, CommandResult>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IAulaRepository _aulaRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private Guid _raizAgregacao;

        public AtualizarMaterialCommandHandler( IMaterialRepository materialRepository,
                                                IAulaRepository aulaRepository,
                                                IMediatorHandler mediatorHandler)
        {
            _materialRepository = materialRepository;
            _aulaRepository = aulaRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<CommandResult> Handle(AtualizarMaterialCommand request, CancellationToken cancellationToken)
        {

            _raizAgregacao = request.RaizAgregacao;
            var materialExistente = await _materialRepository.ObterPorIdAsync(request.Id);

            if (!await ValidarRequisicao(request, materialExistente))
                return request.Resultado;

            materialExistente.AtualizarInformacoes(
                request.Nome,
                request.Descricao,
                request.TipoMaterial,
                request.Url,
                request.IsObrigatorio,
                request.TamanhoBytes,
                request.Extensao,
                request.Ordem);

            await _materialRepository.AtualizarMaterialAsync(materialExistente);

            if (await _materialRepository.UnitOfWork.Commit())
            {
                request.Resultado.Data = true;
            }

            return request.Resultado;
            
        }

        private async Task<bool> ValidarRequisicao(AtualizarMaterialCommand request, Material material)
        {
            request.DefinirValidacao(new AtualizarMaterialCommandValidator().Validate(request));
            if (!request.EhValido())
            {
                foreach (var erro in request.Erros)
                {
                    await _mediatorHandler.PublicarNotificacaoDominio(
                        new DomainNotificacaoRaiz(_raizAgregacao, nameof(Material), erro));
                }
                return false;
            }

            if (material == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Material), "Material não encontrado"));
                return false;
            }

            if (await _materialRepository.ExistePorNomeAsync(material.AulaId, request.Nome, request.Id))
            {
                await _mediatorHandler.PublicarNotificacaoDominio(
                    new DomainNotificacaoRaiz(_raizAgregacao, nameof(Material), "Já existe um material com este nome na aula"));
                return false;
            }

            return true;
        }
    }

}
