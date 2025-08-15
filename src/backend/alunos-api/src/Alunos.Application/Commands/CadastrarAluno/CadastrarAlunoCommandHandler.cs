using MediatR;
using Core.Communication;
using Alunos.Domain.Interfaces;
using Core.Mediator;
using Alunos.Domain.Entities;
using Core.Messages;
using FluentValidation.Results;

namespace Alunos.Application.Commands.CadastrarAluno;
public class CadastrarAlunoCommandHandler(IAlunoRepository alunoRepository, IMediatorHandler mediatorHandler) : IRequestHandler<CadastrarAlunoCommand, CommandResult>
{
    private readonly IAlunoRepository _alunoRepository = alunoRepository;
    private readonly IMediatorHandler _mediatorHandler = mediatorHandler;
    private Guid _raizAgregacao;

    public async Task<CommandResult> Handle(CadastrarAlunoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _raizAgregacao = request.RaizAgregacao;
            if (!await ValidarRequisicao(request)) { return request.Resultado; }

            var aluno = new Aluno(request.Id,
                request.Nome,
                request.Email,
                request.Cpf,
                request.DataNascimento,
                request.Genero,
                request.Cidade,
                request.Estado,
                request.Cep,
                request.Foto);

            await _alunoRepository.AdicionarAsync(aluno);
            if (await _alunoRepository.UnitOfWork.Commit()) { request.Resultado.Data = aluno.Id; }

            return request.Resultado;
        }
        catch (Exception ex)
        {
            request.Validacao.Errors.Add(new ValidationFailure("Exception", $"Erro ao registrar aluno: {ex.Message}"));
            return request.Resultado;
        }
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

        alunoExistente = await _alunoRepository.ObterPorEmailAsync(request.Email);
        if (alunoExistente != null)
        {
            await _mediatorHandler.PublicarNotificacaoDominio(new DomainNotificacaoRaiz(_raizAgregacao, nameof(Aluno), "Já existe um aluno cadastrado com este email."));
            return false;
        }

        return true;
    }

}
