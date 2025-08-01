using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands
{
    public class CategoriaHandler(ICategoriaRepository categoriaRepository) : CommandHandler
        , IRequestHandler<CadastrarCategoriaCommand, CommandResult>
    {
        public async Task<CommandResult> Handle(CadastrarCategoriaCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return request.CommandResult;

            var categoria = new Categoria(
                   request.Nome,
                   request.Descricao,
                   request.Cor,
                   request.IconeUrl,
                   request.Ordem
            );

            if (await categoriaRepository.ExistePorNome(categoria.Nome))
            {
                request.CommandResult.AdicionarErro(nameof(request.Nome), "Já existe uma categoria com este nome.");
                return request.CommandResult;
            }

            categoriaRepository.Adicionar(categoria);
            return await PersistirDados(categoriaRepository.UnitOfWork);
        }
    }
}
