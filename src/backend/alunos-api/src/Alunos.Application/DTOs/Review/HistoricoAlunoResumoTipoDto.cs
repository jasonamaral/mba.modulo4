namespace Alunos.Application.DTOs.Review;

public class HistoricoAlunoResumoTipoDto
{
    public string TipoAcao { get; set; } = string.Empty;

    public string NomeTipoAcao { get; set; } = string.Empty;

    public int Quantidade { get; set; }

    public decimal Percentual { get; set; }

    public DateTime? PrimeiraOcorrencia { get; set; }

    public DateTime? UltimaOcorrencia { get; set; }
}