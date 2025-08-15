using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarCurso;
public class AtualizarCursoCommandHandler(IMediatorHandler mediatorHandler
                                        , ICursoRepository cursoRepository
                                        , ICategoriaRepository categoriaRepository)
    : IRequestHandler<AtualizarCursoCommand, CommandResult>
{
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private readonly ICursoRepository _cursoRepository = cursoRepository;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        var curso = await _cursoRepository.ObterPorIdAsync(request.Id, noTracking: false);

        if (!await ValidarRequisicao(request, curso)) { return request.Resultado; }

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


        await _cursoRepository.Atualizar(curso);
        request.Resultado.Data = await _cursoRepository.UnitOfWork.Commit();
        return request.Resultado;
    }

    private async Task<bool> ValidarRequisicao(AtualizarCursoCommand request, Curso curso)
    {
        request.DefinirValidacao(new AtualizarCursoCommandValidator().Validate(request));

        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
               await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), erro));
            }
            return false;
        }

        if (curso == null)
        {
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Curso não encontrado."));
            return false;
        }

        if (await _cursoRepository.ExistePorNomeAsync(curso.Nome, curso.Id))
        {
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Já existe um curso com este nome."));
            return false;
        }

        if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
        {
            var categoria = await _categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
            if (categoria == null)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Categoria não encontrada"));
                return false;
            }
        }

        return true;
    }
}
