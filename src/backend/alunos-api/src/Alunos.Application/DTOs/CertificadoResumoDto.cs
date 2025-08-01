namespace Alunos.Application.DTOs;

public class CertificadoResumoDto
{
    public Guid Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string NomeCurso { get; set; } = string.Empty;

    public string NomeAluno { get; set; } = string.Empty;

    public DateTime DataEmissao { get; set; }

    public DateTime? DataValidade { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool EstaValido { get; set; }

    public int DiasRestantesValidade { get; set; }
}