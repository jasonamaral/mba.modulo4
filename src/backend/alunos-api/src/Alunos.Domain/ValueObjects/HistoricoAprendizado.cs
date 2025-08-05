using Core.DomainValidations;

namespace Alunos.Domain.ValueObjects;
public class HistoricoAprendizado
{
    #region Atributos
    public Guid Id { get; }
    public Guid MatriculaCursoId { get; }
    public Guid CursoId { get; }
    public Guid AulaId { get; }
    public string NomeAula { get; }
    public byte CargaHoraria { get; }
    public DateTime DataInicio { get; }
    public DateTime? DataTermino { get; }
    #endregion

    #region CTOR
    // EF Compatibility
    protected HistoricoAprendizado() { }

    public HistoricoAprendizado(Guid matriculaCursoId,
        Guid cursoId,
        Guid aulaId,
        string nomeAula,
        byte cargaHoraria,
        DateTime? dataInicio = null,
        DateTime? dataTermino = null)
    {
        Id = Guid.NewGuid();
        MatriculaCursoId = matriculaCursoId;
        CursoId = cursoId;
        AulaId = aulaId;
        NomeAula = nomeAula?.Trim() ?? string.Empty;
        CargaHoraria = cargaHoraria;
        DataInicio = dataInicio ?? DateTime.UtcNow.Date;
        DataTermino = dataTermino;

        ValidarIntegridadeHistoricoAprendizado();
    }
    #endregion

    #region Métodos
    private void ValidarIntegridadeHistoricoAprendizado()
    {
        var validacao = new ResultadoValidacao<HistoricoAprendizado>();

        ValidacaoGuid.DeveSerValido(CursoId, "Identifição do curso não pode ser vazio", validacao);
        ValidacaoGuid.DeveSerValido(AulaId, "Identifição da aula não pode ser vazio", validacao);
        ValidacaoTexto.DevePossuirConteudo(NomeAula, "Nome da aula não pode ser vazio", validacao);
        ValidacaoTexto.DevePossuirTamanho(NomeAula, 5, 100, "Nome da aula deve ter entre 5 e 100 caracteres", validacao);
        ValidacaoData.DeveSerValido(DataInicio, "Data de início é inválida", validacao);
        ValidacaoData.DeveSerMenorQue(DataInicio, DateTime.Now, "Data de início não pode ser superior à data atual", validacao);
        ValidacaoNumerica.DeveSerMaiorQueZero(CargaHoraria, "Carga horária deve ser maior que zero", validacao);
        ValidacaoNumerica.DeveEstarEntre(CargaHoraria, 1, 120, "Carga horária deve estar entre 1 e 120 horas", validacao);

        if (DataTermino.HasValue)
        {
            ValidacaoData.DeveSerValido(DataTermino.Value, "Data de término é inválida", validacao);
            ValidacaoData.DeveSerMaiorQue(DataTermino.Value, DataInicio, "Data de término não pode ser menor que a data de início", validacao);
            ValidacaoData.DeveSerMenorQue(DataTermino.Value, DateTime.Now, "Data de término não pode ser superior à data atual", validacao);
        }

        validacao.DispararExcecaoDominioSeInvalido();
    }
    #endregion

    public override string ToString()
    {
        string conclusao = DataTermino.HasValue ? $"(Término em {DataTermino:dd/MM/yyyy})" : "(Em andamento)";
        return $"Aula {NomeAula} Iniciada em {DataInicio:dd/MM/yyyy} {conclusao}";
    }
}
