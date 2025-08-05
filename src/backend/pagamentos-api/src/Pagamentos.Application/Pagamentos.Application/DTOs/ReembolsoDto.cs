namespace Pagamentos.Application.DTOs
{
    public class ReembolsoDto
    {
        public Guid Id { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public string Motivo { get; set; }
        public string ReembolsoId { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public string MotivoRejeicao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
} 