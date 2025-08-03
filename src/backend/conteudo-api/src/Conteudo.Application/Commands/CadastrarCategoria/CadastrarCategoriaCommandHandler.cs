using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.CadastrarCategoria;
public class CadastrarCategoriaCommandHandler(ICategoriaRepository categoriaRepository, 
    IMediatorHandler mediatorHandler) : IRequestHandler<CadastrarCategoriaCommand, CommandResult>
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(CadastrarCategoriaCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (!await ValidarRequisicao(request)) { return request.CommandResult; }

        var categoria = new Categoria(request.Nome,
            request.Descricao,
            request.Cor,
            request.IconeUrl,
            request.Ordem);

        await categoriaRepository.Adicionar(categoria);
        await categoriaRepository.UnitOfWork.Commit();
        return request.CommandResult;
    }

    private async Task<bool> ValidarRequisicao(CadastrarCategoriaCommand request)
    {
        request.DefinirValidacao(new CadastrarCategoriaCommandValidator().Validate(request));
        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Categoria), erro)).GetAwaiter().GetResult();
            }
            return false;
        }

        if (await _categoriaRepository.ExistePorNome(request.Nome))
        {
            _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Categoria), "Já existe uma categoria com este nome.")).GetAwaiter().GetResult();
            return false;
        }

        return true;
    }
}
