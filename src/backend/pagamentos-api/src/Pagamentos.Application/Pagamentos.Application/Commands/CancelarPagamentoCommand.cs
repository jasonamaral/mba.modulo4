namespace Pagamentos.Application.Commands
{
    public class CancelarPagamentoCommand
    {
        public Guid PagamentoId { get; set; }
        public string Motivo { get; set; }
        public Guid UsuarioId { get; set; }
    }
} 