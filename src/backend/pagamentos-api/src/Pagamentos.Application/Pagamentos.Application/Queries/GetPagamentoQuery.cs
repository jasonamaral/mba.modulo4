namespace Pagamentos.Application.Queries
{
    public class GetPagamentoQuery
    {
        public Guid PagamentoId { get; set; }
        public Guid? UsuarioId { get; set; }

        public GetPagamentoQuery(Guid pagamentoId, Guid? usuarioId = null)
        {
            PagamentoId = pagamentoId;
            UsuarioId = usuarioId;
        }
    }
} 