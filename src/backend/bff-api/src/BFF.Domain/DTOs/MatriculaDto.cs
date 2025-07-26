namespace BFF.Domain.DTOs;

public class MatriculaDto
{
    public Guid Id { get; set; }
    public Guid AlunoId { get; set; }
    public Guid CursoId { get; set; }
    public string CursoNome { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PercentualConclusao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
