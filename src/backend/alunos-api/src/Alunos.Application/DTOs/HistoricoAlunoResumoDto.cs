namespace Alunos.Application.DTOs;

public class HistoricoAlunoResumoDto
{
    public Guid Id { get; set; }

    public string Acao { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string TipoAcao { get; set; } = string.Empty;

    public bool EhRecente { get; set; }

    public DateTime CreatedAt { get; set; }
}