namespace Pagamentos.Application.Queries
{
    public class GetPagamentosByAlunoQuery
    {
        public Guid AlunoId { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
        public string Status { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string MetodoPagamento { get; set; }

        public GetPagamentosByAlunoQuery(Guid alunoId)
        {
            AlunoId = alunoId;
        }
    }
} 