namespace BFF.Domain.DTOs;

public class DashboardAdminDto
{
    public EstatisticasAlunosDto EstatisticasAlunos { get; set; } = new();
    public EstatisticasCursosDto EstatisticasCursos { get; set; } = new();
    public RelatorioVendasDto RelatorioVendas { get; set; } = new();
    public EstatisticasUsuariosDto EstatisticasUsuarios { get; set; } = new();
    public List<CursoPopularDto> CursosPopulares { get; set; } = new();
    public List<VendaRecenteDto> VendasRecentes { get; set; } = new();
}

public class EstatisticasAlunosDto
{
    public int TotalAlunos { get; set; }
    public int AlunosAtivos { get; set; }
    public int AlunosInativos { get; set; }
    public int NovasMatriculasHoje { get; set; }
    public int NovasMatriculasSemana { get; set; }
    public int NovasMatriculasMes { get; set; }
    public decimal TaxaRetencao { get; set; }
}

public class EstatisticasCursosDto
{
    public int TotalCursos { get; set; }
    public int CursosAtivos { get; set; }
    public int CursosInativos { get; set; }
    public decimal MediaAvaliacoes { get; set; }
    public int TotalAulas { get; set; }
    public int HorasConteudo { get; set; }
}

public class RelatorioVendasDto
{
    public decimal VendasHoje { get; set; }
    public decimal VendasSemana { get; set; }
    public decimal VendasMes { get; set; }
    public decimal VendasAno { get; set; }
    public decimal TicketMedio { get; set; }
    public int TotalTransacoes { get; set; }
    public decimal TaxaConversao { get; set; }
}

public class EstatisticasUsuariosDto
{
    public int TotalUsuarios { get; set; }
    public int UsuariosAtivos { get; set; }
    public int UsuariosOnline { get; set; }
    public int AdminsAtivos { get; set; }
    public int AlunosAtivos { get; set; }
}

public class CursoPopularDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalMatriculas { get; set; }
    public decimal Receita { get; set; }
    public decimal MediaAvaliacoes { get; set; }
    public int TotalAvaliacoes { get; set; }
}

public class VendaRecenteDto
{
    public Guid Id { get; set; }
    public string AlunoNome { get; set; } = string.Empty;
    public string CursoNome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataVenda { get; set; }
    public string Status { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = string.Empty;
} 