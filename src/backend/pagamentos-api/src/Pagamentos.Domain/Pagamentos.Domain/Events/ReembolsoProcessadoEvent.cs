using System;

namespace Pagamentos.Domain.Events
{
    public class ReembolsoProcessadoEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string EventType { get; set; } = "ReembolsoProcessado";
        public ReembolsoProcessadoData Data { get; set; }

        public ReembolsoProcessadoEvent(ReembolsoProcessadoData data)
        {
            Data = data;
        }
    }

    public class ReembolsoProcessadoData
    {
        public Guid ReembolsoId { get; set; }
        public Guid PagamentoId { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public decimal Valor { get; set; }
        public string Motivo { get; set; }
        public string ReembolsoExternoId { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
} 