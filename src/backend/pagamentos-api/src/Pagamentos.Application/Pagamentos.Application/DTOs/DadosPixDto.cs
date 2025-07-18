namespace Pagamentos.Application.DTOs
{
    public class DadosPixDto
    {
        public string ChavePix { get; set; }
        public string TipoChave { get; set; } // CPF, CNPJ, Email, Telefone, Aleatoria
        public string QrCode { get; set; }
        public string TxId { get; set; }
    }
} 