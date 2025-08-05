namespace Alunos.Application.DTOs.Review;

public class HistoricoAlunoEstatisticasDto
{
    public int TotalLogins { get; set; }

    public int TotalAcessosAulas { get; set; }

    public int TotalMatriculas { get; set; }

    public int TotalConclusoes { get; set; }

    public int TotalCertificacoes { get; set; }

    public decimal MediaAcoesPorDia { get; set; }

    public string DiaSemanaComMaisAtividade { get; set; } = string.Empty;

    public string HorarioComMaisAtividade { get; set; } = string.Empty;

    public DateTime? PrimeiroAcesso { get; set; }

    public DateTime? UltimoAcesso { get; set; }

    public int DiasDesdeUltimoAcesso { get; set; }
}