namespace Pagamentos.Application.ViewModels
{
    public class TransacaoViewModel
    {
        public Guid Id { get; set; }

        public string CodigoAutorizacao { get; set; } = string.Empty;
        public string BandeiraCartao { get; set; } = string.Empty;
        public string StatusTransacao { get; set; } = string.Empty;

        public decimal ValorTotal { get; set; }
    }
}
