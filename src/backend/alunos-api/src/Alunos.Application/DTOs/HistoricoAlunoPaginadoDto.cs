namespace Alunos.Application.DTOs;

public class HistoricoAlunoPaginadoDto
{
    public List<HistoricoAlunoDto> Historicos { get; set; } = new();

    public int PaginaAtual { get; set; }

    public int TotalPaginas { get; set; }

    public int TotalRegistros { get; set; }

    public int TamanhoPagina { get; set; }

    public bool TemPaginaAnterior { get; set; }

    public bool TemProximaPagina { get; set; }
}