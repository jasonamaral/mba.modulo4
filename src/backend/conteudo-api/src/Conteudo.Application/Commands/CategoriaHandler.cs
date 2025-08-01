using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.Entities;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Conteudo.Application.Commands
{
    public class CategoriaHandler(ICategoriaRepository categoriaRepository) : CommandHandler, IRequestHandler<CadastrarCategoriaCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(CadastrarCategoriaCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return request.ValidationResult;

            var categoria = new Categoria(
                   request.Nome,
                   request.Descricao,
                   request.Cor,
                   request.IconeUrl,
                   request.Ordem
            );

            if (await categoriaRepository.ExistePorNome(categoria.Nome))
            {
                request.ValidationResult.Errors.Add(new ValidationFailure(nameof(request.Nome), "Já existe uma categoria com este nome."));
                return request.ValidationResult;
            }

            categoriaRepository.Adicionar(categoria);
            return await PersistirDados(categoriaRepository.UnitOfWork);
        }
    }
}
