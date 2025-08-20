namespace BFF.Domain.DTOs.Pagamentos.Response
{
    public class TransacaoDto
    {
        public Guid Id { get; set; }
        public string CodigoAutorizacao { get; set; } = string.Empty;
        public string BandeiraCartao { get; set; } = string.Empty;
        public string StatusTransacao { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
    }
}
