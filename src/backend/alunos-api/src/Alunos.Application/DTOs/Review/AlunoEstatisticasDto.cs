namespace Alunos.Application.DTOs.Review;

public class AlunoEstatisticasDto
{
    public int TotalMatriculas { get; set; }

    public int MatriculasAtivas { get; set; }

    public int CursosConcluidos { get; set; }

    public int CertificadosEmitidos { get; set; }

    public decimal HorasEstudoTotal { get; set; }

    public decimal MediaNotas { get; set; }

    public decimal PercentualConclusaoMedio { get; set; }

    public DateTime? UltimoAcesso { get; set; }
}