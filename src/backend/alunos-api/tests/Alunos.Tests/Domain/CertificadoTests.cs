using Alunos.Domain.Entities;
using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Tests.Domain;

public class CertificadoTests
{
    #region Helpers

    private static readonly Guid _matriculaIdValido = Guid.Parse("0044bf68-a9ee-462f-9e43-d12f8ffef93e");
    private static readonly string _nomeCursoValido = "Curso de Testes";
    private static readonly short _cargaHorariaValida = 40;
    private static readonly byte _notaFinalValida = 8;
    private static readonly string _pathValido = "/certificados/teste.pdf";
    private static readonly string _instrutorValido = "Eduardo Pires";

    private Certificado CriarCertificadoValido() => new(_matriculaIdValido, _nomeCursoValido, DateTime.Now, null, _cargaHorariaValida, _notaFinalValida, _pathValido, _instrutorValido);

    #endregion Helpers

    #region Construtores

    [Fact]
    public void Deve_criar_certificado_valido()
    {
        var certificado = CriarCertificadoValido();

        certificado.Should().NotBeNull();
        certificado.MatriculaCursoId.Should().Be(_matriculaIdValido);
        certificado.NomeCurso.Should().Be(_nomeCursoValido);
        certificado.CargaHoraria.Should().Be(_cargaHorariaValida);
        certificado.NotaFinal.Should().Be(_notaFinalValida);
        certificado.PathCertificado.Should().Be(_pathValido);
        certificado.NomeInstrutor.Should().Be(_instrutorValido);
        certificado.DataSolicitacao.Date.Should().Be(DateTime.Now.Date);
    }

    [Theory]
    [InlineData("", "Path do certificado não pode ser nulo ou vazio")]
    [InlineData(null, "Path do certificado não pode ser nulo ou vazio")]
    public void Nao_deve_criar_certificado_com_path_invalido(string path, string mensagem)
    {
        Action act = () => new Certificado(_matriculaIdValido, _nomeCursoValido, DateTime.Now, null, _cargaHorariaValida, _notaFinalValida, path, _instrutorValido);
        act.Should().Throw<DomainException>().WithMessage($"*{mensagem}*");
    }

    [Fact]
    public void Nao_deve_criar_certificado_com_carga_horaria_zero()
    {
        Action act = () => new Certificado(_matriculaIdValido, _nomeCursoValido, DateTime.Now, null, 0, _notaFinalValida, _pathValido, _instrutorValido);
        act.Should().Throw<DomainException>().WithMessage("*Carga horária deve ser maior que zero*");
    }

    [Fact]
    public void Nao_deve_criar_certificado_com_nota_maior_que_10()
    {
        Action act = () => new Certificado(_matriculaIdValido, _nomeCursoValido, DateTime.Now, null, _cargaHorariaValida, 11, _pathValido, _instrutorValido);
        act.Should().Throw<DomainException>().WithMessage("*Nota final deve estar entre 0 e 10*");
    }

    [Fact]
    public void Nao_deve_criar_certificado_com_nome_curso_vazio()
    {
        Action act = () => new Certificado(_matriculaIdValido, "", DateTime.Now, null, _cargaHorariaValida, _notaFinalValida, _pathValido, _instrutorValido);
        act.Should().Throw<DomainException>().WithMessage("*Nome do curso não pode ser nulo ou vazio*");
    }

    #endregion Construtores

    #region Atualizacoes

    [Fact]
    public void Deve_atualizar_carga_horaria()
    {
        var certificado = CriarCertificadoValido();
        certificado.AtualizarCargaHoraria(80);
        certificado.CargaHoraria.Should().Be(80);
    }

    [Fact]
    public void Nao_deve_atualizar_path_se_emitido()
    {
        var certificado = CriarCertificadoValido();
        certificado.AtualizarDataEmissao(DateTime.Now);
        Action act = () => certificado.AtualizarPathCertificado("/novo.pdf");
        act.Should().Throw<DomainException>().WithMessage("*Certificado foi emitido e não pode sofrer alterações*");
    }

    [Fact]
    public void Nao_deve_atualizar_nota_final_invalida()
    {
        var certificado = CriarCertificadoValido();
        Action act = () => certificado.AtualizarNotaFinal(15);
        act.Should().Throw<DomainException>().WithMessage("*Nota final deve estar entre 0 e 10*");
    }

    #endregion Atualizacoes

    #region Overrides

    [Fact]
    public void ToString_deve_retornar_formatado()
    {
        var certificado = CriarCertificadoValido();
        var texto = certificado.ToString();

        texto.Should().Contain(certificado.NomeCurso)
              .And.Contain(certificado.MatriculaCursoId.ToString("D"))
              .And.Contain(certificado.CargaHoraria.ToString())
              .And.Contain(certificado.NotaFinal.ToString());
    }

    #endregion Overrides
}
