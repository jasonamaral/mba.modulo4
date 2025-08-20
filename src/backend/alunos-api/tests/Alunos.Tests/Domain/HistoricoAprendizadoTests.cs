using Alunos.Domain.ValueObjects;
using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Tests.Domain;

public class HistoricoAprendizadoTests
{
    #region Helpers

    private static readonly Guid _matriculaIdValido = Guid.NewGuid();
    private static readonly Guid _cursoIdValido = Guid.NewGuid();
    private static readonly Guid _aulaIdValido = Guid.NewGuid();
    private static readonly string _nomeAulaValido = "Introdução à Lógica de Programação";
    private static readonly byte _cargaHorariaValida = 20;

    private HistoricoAprendizado CriarHistoricoValido(DateTime? inicio = null, DateTime? termino = null) => new(_matriculaIdValido, _cursoIdValido, _aulaIdValido, _nomeAulaValido, _cargaHorariaValida, inicio ?? new DateTime(2024, 1, 1), termino);

    #endregion Helpers

    #region Construtores

    [Fact]
    public void Deve_criar_historico_valido()
    {
        var historico = CriarHistoricoValido();

        historico.Should().NotBeNull();
        historico.CursoId.Should().Be(_cursoIdValido);
        historico.AulaId.Should().Be(_aulaIdValido);
        historico.NomeAula.Should().Be(_nomeAulaValido);
        historico.CargaHoraria.Should().Be(_cargaHorariaValida);
        historico.DataInicio.Date.Should().Be(new DateTime(2024, 1, 1));
        historico.DataTermino.Should().BeNull();
    }

    [Theory]
    [InlineData("", "*Nome da aula não pode ser vazio*")]
    [InlineData("A", "*Nome da aula deve ter entre 5 e 100 caracteres*")]
    public void Nao_deve_criar_historico_com_nome_invalido(string nome, string mensagemEsperada)
    {
        Action act = () => new HistoricoAprendizado(_matriculaIdValido, _cursoIdValido, _aulaIdValido, nome, _cargaHorariaValida);
        act.Should().Throw<DomainException>().WithMessage(mensagemEsperada);
    }

    [Fact]
    public void Nao_deve_criar_historico_com_carga_horaria_zero()
    {
        Action act = () => new HistoricoAprendizado(_matriculaIdValido, _cursoIdValido, _aulaIdValido, _nomeAulaValido, 0);
        act.Should().Throw<DomainException>().WithMessage("*Carga horária deve ser maior que zero*");
    }

    [Fact]
    public void Nao_deve_criar_historico_com_data_inicio_futura()
    {
        var dataFutura = DateTime.Now.AddDays(1);
        Action act = () => CriarHistoricoValido(inicio: dataFutura);
        act.Should().Throw<DomainException>().WithMessage("*Data de início não pode ser superior à data atual*");
    }

    [Fact]
    public void Nao_deve_criar_historico_com_data_termino_anterior_ao_inicio()
    {
        var inicio = DateTime.Now.Date;
        var termino = inicio.AddDays(-1);
        Action act = () => CriarHistoricoValido(inicio, termino);
        act.Should().Throw<DomainException>().WithMessage("*Data de término não pode ser menor que a data de início*");
    }

    [Fact]
    public void Nao_deve_criar_historico_com_data_termino_futura()
    {
        var termino = DateTime.Now.AddDays(1);
        Action act = () => CriarHistoricoValido(DateTime.Now.Date, termino);
        act.Should().Throw<DomainException>().WithMessage("*Data de término não pode ser superior à data atual*");
    }

    #endregion Construtores

    #region Overrides

    [Fact]
    public void ToString_deve_retornar_formatado_para_concluido()
    {
        var inicio = DateTime.Now.AddDays(-10);
        var termino = DateTime.Now.AddDays(-1);
        var historico = CriarHistoricoValido(inicio, termino);

        var texto = historico.ToString();
        texto.Should().Contain("Término em")
              .And.Contain(historico.NomeAula);
    }

    [Fact]
    public void ToString_deve_retornar_formatado_para_em_andamento()
    {
        var historico = CriarHistoricoValido();

        var texto = historico.ToString();
        texto.Should().Contain("Em andamento")
              .And.Contain(historico.NomeAula);
    }

    #endregion Overrides

    #region Overrides

    [Fact]
    public void ToString_deve_conter_nome_aula_e_em_andamento()
    {
        var historico = CriarHistoricoValido();
        var texto = historico.ToString();

        texto.Should().Contain("Iniciada em")
              .And.Contain("(Em andamento)");
    }

    [Fact]
    public void ToString_deve_conter_nome_aula_e_data_finalizadao()
    {
        var termino = DateTime.Now.Date;
        var historico = CriarHistoricoValido(termino: termino);
        var texto = historico.ToString();

        texto.Should().Contain("Iniciada em")
              .And.Contain(termino.ToString("dd/MM/yyyy"));
    }

    #endregion Overrides
}
