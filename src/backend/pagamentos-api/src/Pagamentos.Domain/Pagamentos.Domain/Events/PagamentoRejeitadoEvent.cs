using System;

namespace Pagamentos.Domain.Events
{
    public class PagamentoRejeitadoEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; } = "PagamentoRejeitado";
        public PagamentoRejeitadoData Data { get; set; }

        public PagamentoRejeitadoEvent(PagamentoRejeitadoData data)
        {
            Data = data;
        }
    }

    public class PagamentoRejeitadoData
    {
        public Guid PagamentoId { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public decimal Valor { get; set; }
        public string MetodoPagamento { get; set; }
        public string GatewayPagamento { get; set; }
        public string MotivoRejeicao { get; set; }
        public DateTime DataRejeicao { get; set; }
    }
} 