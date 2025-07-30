namespace Alunos.Application.DTOs;

public class CertificadoDto
{
    public Guid Id { get; set; }

    public Guid MatriculaId { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string NomeCurso { get; set; } = string.Empty;

    public string NomeAluno { get; set; } = string.Empty;

    public DateTime DataEmissao { get; set; }

    public DateTime? DataValidade { get; set; }

    public int CargaHoraria { get; set; }

    public decimal? NotaFinal { get; set; }

    public string Status { get; set; } = string.Empty;

    public string UrlArquivo { get; set; } = string.Empty;

    public string HashValidacao { get; set; } = string.Empty;

    public string Observacoes { get; set; } = string.Empty;

    public string NomeInstrutor { get; set; } = string.Empty;

    public bool EstaValido { get; set; }

    public bool EstaExpirado { get; set; }

    public int DiasRestantesValidade { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}