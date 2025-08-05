using Conteudo.Domain.Entities;
using Conteudo.Domain.Interfaces.Repositories;
using Conteudo.Domain.ValueObjects;
using Core.Communication;
using Core.Mediator;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarCurso
{
    public class AtualizarCursoCommandHandler(IMediatorHandler mediatorHandler,
        ICursoRepository cursoRepository,
        ICategoriaRepository categoriaRepository) : IRequestHandler<AtualizarCursoCommand, CommandResult>
    {
        private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
        private readonly ICursoRepository _cursoRepository = cursoRepository;
        private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;
        private Guid _raizAgregacao;

        public async Task<CommandResult> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
        {
            _raizAgregacao = request.RaizAgregacao;
            var curso = await _cursoRepository.ObterPorIdAsync(request.Id, noTracking: false);

            if (!await ValidarRequisicao(request, curso)) { return request.CommandResult; }

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
            await _cursoRepository.UnitOfWork.Commit();
            return request.CommandResult;
        }

        private async Task<bool> ValidarRequisicao(AtualizarCursoCommand request, Curso curso)
        {
            request.DefinirValidacao(new AtualizarCursoCommandValidator().Validate(request));

            if (!request.EhValido())
            {
                foreach (var erro in request.Erros)
                {
                    _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), erro)).GetAwaiter().GetResult();
                }
                return false;
            }

            if (curso == null)
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Curso não encontrado.")).GetAwaiter().GetResult();
                return false;
            }

            if (await _cursoRepository.ExistePorNomeAsync(curso.Nome, curso.Id))
            {
                _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Já existe um curso com este nome.")).GetAwaiter().GetResult();
                return false;
            }

            if (curso.CategoriaId != null && curso.CategoriaId != Guid.Empty)
            {
                var categoria = await _categoriaRepository.ObterPorIdAsync((Guid)curso.CategoriaId);
                if (categoria == null)
                {
                    _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Curso), "Categoria não encontrada")).GetAwaiter().GetResult();
                    return false;
                }
            }

            return true;
        }
    }
}
