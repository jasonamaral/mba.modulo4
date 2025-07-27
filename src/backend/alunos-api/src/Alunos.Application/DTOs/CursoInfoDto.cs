namespace Alunos.Application.DTOs;

public class CursoInfoDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int CargaHoraria { get; set; }

    public string Instrutor { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Nivel { get; set; } = string.Empty;

    public string ImagemUrl { get; set; } = string.Empty;
}