using System.ComponentModel.DataAnnotations;

namespace Alunos.Application.DTOs;

public class HistoricoAlunoFiltroDto
{
    public Guid? AlunoId { get; set; }

    public string TipoAcao { get; set; } = string.Empty;

    public DateTime? DataInicial { get; set; }

    public DateTime? DataFinal { get; set; }

    public string EnderecoIP { get; set; } = string.Empty;

    public bool? ApenasRecentes { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
    public int Pagina { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
    public int TamanhoPagina { get; set; } = 20;

    public string CampoOrdenacao { get; set; } = "CreatedAt";

    public string DirecaoOrdenacao { get; set; } = "desc";
}