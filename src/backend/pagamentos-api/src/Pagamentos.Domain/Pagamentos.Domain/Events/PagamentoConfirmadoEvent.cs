using System;

namespace Pagamentos.Domain.Events
{
    public class PagamentoConfirmadoEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; } = "PagamentoConfirmado";
        public PagamentoConfirmadoData Data { get; set; }

        public PagamentoConfirmadoEvent(PagamentoConfirmadoData data)
        {
            Data = data;
        }
    }

    public class PagamentoConfirmadoData
    {
        public Guid PagamentoId { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; }
        public string GatewayPagamento { get; set; }
        public string TransacaoId { get; set; }
        public DateTime DataPagamento { get; set; }
    }
} 