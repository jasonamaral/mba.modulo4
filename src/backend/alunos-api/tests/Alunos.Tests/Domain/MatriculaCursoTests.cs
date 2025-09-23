using Alunos.Domain.Entities;
using Alunos.Domain.Enumerators;
using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Tests.Domain;

public class MatriculaCursoTests
{
    private static readonly Guid _alunoId = Guid.NewGuid();
    private static readonly Guid _cursoId = Guid.NewGuid();
    private static readonly string _nomeCurso = "Curso Completo de Testes Automatizados";
    private static readonly decimal _valor = 500m;
    private static readonly string _observacao = "Observações iniciais";

    private MatriculaCurso CriarMatriculaValida() => new(_alunoId, _cursoId, _nomeCurso, _valor, _observacao);

    [Fact]
    public void Deve_criar_matricula_valida()
    {
        var matricula = CriarMatriculaValida();

        matricula.Should().NotBeNull();
        matricula.AlunoId.Should().Be(_alunoId);
        matricula.CursoId.Should().Be(_cursoId);
        matricula.Valor.Should().Be(_valor);
        matricula.Observacao.Should().Be(_observacao);
        matricula.EstadoMatricula.Should().Be(EstadoMatriculaCurso.PendentePagamento);
    }

    [Fact]
    public void Nao_deve_criar_matricula_com_nome_curso_invalido()
    {
        Action act = () => new MatriculaCurso(_alunoId, _cursoId, "Curto", _valor, _observacao);
        act.Should().Throw<DomainException>().WithMessage("*Nome do curso deve ter entre 10 e 200 caracteres*");
    }

    [Fact]
    public void Nao_deve_criar_matricula_com_valor_zero()
    {
        Action act = () => new MatriculaCurso(_alunoId, _cursoId, _nomeCurso, 0, _observacao);
        act.Should().Throw<DomainException>().WithMessage("*Valor da matrícula deve ser maior que zero*");
    }

    [Fact]
    public void Deve_registrar_pagamento()
    {
        var matricula = CriarMatriculaValida();
        matricula.RegistrarPagamentoMatricula();

        matricula.EstadoMatricula.Should().Be(EstadoMatriculaCurso.PagamentoRealizado);
    }

    [Fact]
    public void Deve_registrar_abandono()
    {
        var matricula = CriarMatriculaValida();
        matricula.RegistrarAbandonoMatricula();

        matricula.EstadoMatricula.Should().Be(EstadoMatriculaCurso.Abandonado);
    }

    [Fact]
    public void Nao_deve_concluir_curso_se_matricula_abandonada()
    {
        var matricula = CriarMatriculaValida();
        matricula.RegistrarAbandonoMatricula();

        Action act = () => matricula.ConcluirCurso();
        act.Should().Throw<DomainException>().WithMessage("*Não é possível concluir um curso com estado de pagamento abandonado*");
    }

    [Fact]
    public void Nao_deve_registrar_historico_aprendizado_se_abandonado()
    {
        var matricula = CriarMatriculaValida();
        matricula.RegistrarAbandonoMatricula();

        Action act = () => matricula.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Aula de Teste Completa", 5);
        act.Should().Throw<DomainException>().WithMessage("*Matrícula não está disponível para registrar histórico de aprendizado*");
    }

    [Fact]
    public void Nao_deve_solicitar_certificado_se_curso_nao_concluido()
    {
        var matricula = CriarMatriculaValida();
        matricula.RegistrarPagamentoMatricula();

        Action act = () => matricula.RequisitarCertificadoConclusao(8, "/certificado.pdf", "Instrutor Teste");
        act.Should().Throw<DomainException>().WithMessage("*Certificado só pode ser solicitado após a conclusão do curso*");
    }

    [Fact]
    public void ToString_deve_conter_nome_ids_e_status()
    {
        var matricula = CriarMatriculaValida();
        var texto = matricula.ToString();

        texto.Should().Contain(matricula.AlunoId.ToString())
              .And.Contain(matricula.CursoId.ToString())
              .And.Contain("Concluído?");
    }
}
