namespace BFF.Domain.DTOs;

public class CursoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int CargaHoraria { get; set; }
    public int TotalAulas { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ImagemCapa { get; set; } = string.Empty;
    public List<AulaDto> Aulas { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
