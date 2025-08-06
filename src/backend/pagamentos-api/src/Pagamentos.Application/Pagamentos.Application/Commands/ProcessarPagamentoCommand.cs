using Pagamentos.Application.DTOs;

namespace Pagamentos.Application.Commands
{
    public class ProcessarPagamentoCommand
    {
        // Campos existentes preservados para compatibilidade
        public Guid MatriculaCursoId { get; set; }
        public decimal Valor { get; set; }
        public string NumeroCartao { get; set; }
        public string NomeTitularCartao { get; set; }
        public string ValidadeCartao { get; set; }
        public string CvvCartao { get; set; }
        
        // Novos campos adicionados
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public string MetodoPagamento { get; set; } // CartaoCredito, CartaoDebito, PIX, Boleto, PayPal
        public string GatewayPagamento { get; set; } // MercadoPago, Stripe, PayPal, Pix
        public DateTime? DataVencimento { get; set; }
        public DadosPixDto DadosPix { get; set; }
        public DadosBoletoDto DadosBoleto { get; set; }
        
        // Dados adicionais para processamento
        public string CallbackUrl { get; set; }
        public string DescricaoPagamento { get; set; }
        public string EmailPagador { get; set; }
        public string TelefonePagador { get; set; }
        public string DocumentoPagador { get; set; }
    }
} 