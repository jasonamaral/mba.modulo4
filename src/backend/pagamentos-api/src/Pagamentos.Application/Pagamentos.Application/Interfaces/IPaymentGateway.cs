using System.Threading.Tasks;

namespace Pagamentos.Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        Task<RefundResult> ProcessRefundAsync(RefundRequest request);
        Task<PaymentStatus> GetPaymentStatusAsync(string transactionId);
        Task<bool> ValidateWebhookAsync(string payload, string signature);
        string GetGatewayName();
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string AuthorizationCode { get; set; }
        public decimal Amount { get; set; }
        public string ResponseData { get; set; }
    }

    public class RefundResult
    {
        public bool Success { get; set; }
        public string RefundId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public decimal Amount { get; set; }
        public string ResponseData { get; set; }
    }

    public class PaymentStatus
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public decimal? Amount { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PaymentRequest
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string CallbackUrl { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerDocument { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        
        // Dados do cartão
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CardExpiry { get; set; }
        public string CardCvv { get; set; }
        
        // Dados PIX
        public string PixKey { get; set; }
        public string PixKeyType { get; set; }
        
        // Dados do boleto
        public string BoletoExpiration { get; set; }
        public string BoletoInstructions { get; set; }
    }

    public class RefundRequest
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
} 