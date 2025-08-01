using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Conteudo.Application.Commands
{
    public class CursoHandler(ICursoRepository cursoRepository, ICategoriaRepository categoriaRepository) : CommandHandler
        , IRequestHandler<CadastrarCursoCommand, ValidationResult>
        , IRequestHandler<AtualizarCursoCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(CadastrarCursoCommand request, CancellationToken cancellationToken)
        { 
            if (!request.EhValido())
                return request.ValidationResult;

            var curso = new Curso(request.Nome,
                                  request.Valor,
                                  new ConteudoProgramatico(request.Resumo, 
                                                          request.Descricao, 
                                                          request.Objetivos, 
                                                          request.PreRequisitos, 
                                                          request.PublicoAlvo, 
                                                          request.Metodologia, 
                                                          request.Recursos, 
                                                          request.Avaliacao, 
                                                          request.Bibliografia),
                                  request.DuracaoHoras,
                                  request.Nivel,
                                  request.Instrutor,
                                  request.VagasMaximas,
                                  request.ImagemUrl, 
                                  request.ValidoAte, 
                                  request.CategoriaId);

            if (await cursoRepository.ExistePorNomeAsync(curso.Nome))
            {
                request.ValidationResult.Errors.Add(new ValidationFailure(nameof(request.Nome), "Já existe um curso com este nome."));
                return request.ValidationResult;
            }

            if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
            {
                var categoria = await categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
                if (categoria == null)
                {
                    request.ValidationResult.Errors.Add(new ValidationFailure(nameof(curso.CategoriaId), "Categoria não encontrada"));
                    return request.ValidationResult;
                }
            }
            
            cursoRepository.Adicionar(curso);
            return await PersistirDados(cursoRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return request.ValidationResult;

            var curso = await cursoRepository.ObterPorIdAsync(request.Id, noTracking: false);
            if (curso == null)
            {
                request.ValidationResult.Errors.Add(new ValidationFailure(nameof(request.Id), "Curso não encontrado."));
                return request.ValidationResult;
            }
            curso.AtualizarInformacoes(request.Nome,
                            request.Valor,
                            new ConteudoProgramatico(request.Resumo, 
                                                    request.Descricao, 
                                                    request.Objetivos, 
                                                    request.PreRequisitos, 
                                                    request.PublicoAlvo, 
                                                    request.Metodologia, 
                                                    request.Recursos, 
                                                    request.Avaliacao, 
                                                    request.Bibliografia),
                            request.DuracaoHoras,
                            request.Nivel,
                            request.Instrutor,
                            request.VagasMaximas,
                            request.ImagemUrl, 
                            request.ValidoAte, 
                            request.CategoriaId);

            if (await cursoRepository.ExistePorNomeAsync(curso.Nome, curso.Id))
            {
                request.ValidationResult.Errors.Add(new ValidationFailure(nameof(request.Nome), "Já existe um curso com este nome."));
                return request.ValidationResult;
            }

            if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
            {
                var categoria = await categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
                if (categoria == null)
                {
                    request.ValidationResult.Errors.Add(new ValidationFailure(nameof(curso.CategoriaId), "Categoria não encontrada"));
                    return request.ValidationResult;
                }
            }

            cursoRepository.Atualizar(curso);
            return await PersistirDados(cursoRepository.UnitOfWork);
        }
    }
}
