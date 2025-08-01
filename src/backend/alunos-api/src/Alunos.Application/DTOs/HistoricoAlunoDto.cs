namespace Alunos.Application.DTOs;

public class HistoricoAlunoDto
{
    public Guid Id { get; set; }

    public Guid AlunoId { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public string Acao { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string DetalhesJson { get; set; } = string.Empty;

    public string TipoAcao { get; set; } = string.Empty;

    public Guid? UsuarioId { get; set; }

    public string NomeUsuario { get; set; } = string.Empty;

    public string EnderecoIP { get; set; } = string.Empty;

    public string UserAgent { get; set; } = string.Empty;

    public bool EhRecente { get; set; }

    public DateTime CreatedAt { get; set; }
}
