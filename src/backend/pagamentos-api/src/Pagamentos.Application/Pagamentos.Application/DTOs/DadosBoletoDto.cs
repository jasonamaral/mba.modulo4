using System;

namespace Pagamentos.Application.DTOs
{
    public class DadosBoletoDto
    {
        public string NomePagador { get; set; }
        public string CpfCnpjPagador { get; set; }
        public string EnderecoPagador { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public string NossoNumero { get; set; }
        public string LinhaDigitavel { get; set; }
        public string CodigoBarras { get; set; }
        public string UrlPdf { get; set; }
    }
} 