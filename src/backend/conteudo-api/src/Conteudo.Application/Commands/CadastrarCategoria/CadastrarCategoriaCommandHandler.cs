using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.CadastrarCategoria;
public class CadastrarCategoriaCommandHandler(ICategoriaRepository categoriaRepository
                                            , IMediatorHandler mediatorHandler) 
    : IRequestHandler<CadastrarCategoriaCommand, CommandResult>
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(CadastrarCategoriaCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!await ValidarRequisicao(request)) { return request.Resultado; }

        var categoria = new Categoria(request.Nome,
                                    request.Descricao,
                                    request.Cor,
                                    request.IconeUrl,
                                    request.Ordem);

        _categoriaRepository.Adicionar(categoria);

        if (await _categoriaRepository.UnitOfWork.Commit())
            request.Resultado.Data = categoria.Id;

        return request.Resultado;
    }

    private async Task<bool> ValidarRequisicao(CadastrarCategoriaCommand request)
    {
        request.DefinirValidacao(new CadastrarCategoriaCommandValidator().Validate(request));
        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
               await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Categoria), erro));
            }
            return false;
        }

        if (await _categoriaRepository.ExistePorNome(request.Nome))
        {
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Categoria), "Já existe uma categoria com este nome."));
            return false;
        }

        return true;
    }
}
