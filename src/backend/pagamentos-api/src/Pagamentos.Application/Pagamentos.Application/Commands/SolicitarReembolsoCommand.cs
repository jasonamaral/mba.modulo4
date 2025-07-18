using System;

namespace Pagamentos.Application.Commands
{
    public class SolicitarReembolsoCommand
    {
        public Guid PagamentoId { get; set; }
        public decimal Valor { get; set; }
        public string Motivo { get; set; }
        public Guid UsuarioId { get; set; }
        public string TipoReembolso { get; set; } // Parcial, Total
        public string Observacoes { get; set; }
    }
} 