using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Communication;
using Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Conteudo.Application.Commands
{
    public class CursoHandler(ICursoRepository cursoRepository, ICategoriaRepository categoriaRepository) : CommandHandler
        , IRequestHandler<CadastrarCursoCommand, CommandResult>
        , IRequestHandler<AtualizarCursoCommand, CommandResult>
    {
        public async Task<CommandResult> Handle(CadastrarCursoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return request.CommandResult;

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
                request.CommandResult.AdicionarErro(nameof(request.Nome), "Já existe um curso com este nome.");
                return request.CommandResult;
            }

            if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
            {
                var categoria = await categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
                if (categoria == null)
                {
                    request.CommandResult.AdicionarErro(nameof(curso.CategoriaId), "Categoria não encontrada");
                    return request.CommandResult;
                }
            }

            cursoRepository.Adicionar(curso);
            var result = await PersistirDados(cursoRepository.UnitOfWork);

            if (result.Success)
            {
                result.Data = curso.Id;
            }
            return result;
        }

        public async Task<CommandResult> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
                return request.CommandResult;

            var curso = await cursoRepository.ObterPorIdAsync(request.Id, noTracking: false);
            if (curso == null)
            {
                request.CommandResult.AdicionarErro(nameof(request.Id), "Curso não encontrado.");
                return request.CommandResult;
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
                request.CommandResult.AdicionarErro(nameof(request.Nome), "Já existe um curso com este nome.");
                return request.CommandResult;
            }

            if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
            {
                var categoria = await categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
                if (categoria == null)
                {
                    request.CommandResult.AdicionarErro(nameof(curso.CategoriaId), "Categoria não encontrada");
                    return request.CommandResult;
                }
            }

            cursoRepository.Atualizar(curso);
            return await PersistirDados(cursoRepository.UnitOfWork);
        }
    }
}
