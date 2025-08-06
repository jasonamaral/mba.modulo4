using MediatR;
using Plataforma.Educacao.Aluno.Application.Commands.CadastrarAluno;
using Core.Communication;
using Alunos.Domain.Interfaces;
using Core.Mediator;
using Alunos.Domain.Entities;
using Core.Messages;

namespace Alunos.Application.Commands.CadastrarAluno;
public class CadastrarAlunoCommandHandler(IAlunoRepository alunoRepository, IMediatorHandler mediatorHandler) : IRequestHandler<CadastrarAlunoCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(CadastrarAlunoCommand request, CancellationToken cancellationToken)
    {
        _raizAgregacao = request.RaizAgregacao;
        if (! await ValidarRequisicao(request)) { return request.CommandResult; }



        var aluno = new Aluno(request.Id, request.Nome, request.Email, request.Cpf, request.DataNascimento);

        await _alunoRepository.AdicionarAsync(aluno);
        await _alunoRepository.UnitOfWork.Commit();
        request.CommandResult.Data = aluno.Id;
        return request.CommandResult;
    }

    private async Task<bool> ValidarRequisicao(CadastrarAlunoCommand request)
    {
        request.DefinirValidacao(new CadastrarAlunoCommandValidator().Validate(request));
        if (!request.EhValido())
        {
            foreach (var erro in request.Erros)
            {
                await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aluno), erro));
            }
            return false;
        }

        var alunoExistente = await _alunoRepository.ObterPorIdAsync(request.Id);
        if (alunoExistente != null)
        {
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aluno), "Já existe um aluno cadastrado com este código."));
            return false;
        }

        return true;
    }

}
