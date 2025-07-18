using System;

namespace Pagamentos.Application.DTOs
{
    public class PagamentoDto
    {
        public Guid Id { get; set; }
        public Guid MatriculaId { get; set; }
        public Guid AlunoId { get; set; }
        public Guid CursoId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public string MetodoPagamento { get; set; }
        public string TransacaoId { get; set; }
        public string GatewayPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
        
        // Dados do cartão (mascarados para segurança)
        public string NumeroCartaoMascarado { get; set; }
        public string NomeTitularCartao { get; set; }
        
        // Dados PIX
        public string ChavePix { get; set; }
        public string QrCodePix { get; set; }
        
        // Dados Boleto
        public string LinhaDigitavel { get; set; }
        public string CodigoBarras { get; set; }
    }
} 