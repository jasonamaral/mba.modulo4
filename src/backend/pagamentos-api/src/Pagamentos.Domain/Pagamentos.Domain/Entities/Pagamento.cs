using Pagamentos.Domain.Common;

namespace Pagamentos.Domain.Entities
{
    public class Pagamento : Entidade, IRaizAgregacao
    {
        // Campos existentes preservados
        public Guid MatriculaId { get; private set; }
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public decimal Valor { get; private set; }
        
        // Novos campos adicionados
        public string Status { get; private set; }
        public string MetodoPagamento { get; private set; }
        public string TransacaoId { get; private set; }
        public string GatewayPagamento { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public DateTime? DataVencimento { get; private set; }
        
        // Dados do cartão (quando aplicável)
        public string NumeroCartao { get; private set; }
        public string NomeTitularCartao { get; private set; }
        public string ValidadeCartao { get; private set; }
        public string CvvCartao { get; private set; }
        
        // Dados PIX (quando aplicável)
        public string ChavePix { get; private set; }
        public string QrCodePix { get; private set; }
        
        // Dados Boleto (quando aplicável)
        public string LinhaDigitavel { get; private set; }
        public string CodigoBarras { get; private set; }
        
        private Pagamento() : base()
        {
        }

        public Pagamento(
            Guid matriculaId,
            Guid alunoId,
            Guid cursoId,
            decimal valor,
            string metodoPagamento,
            string gatewayPagamento,
            DateTime? dataVencimento = null) : base()
        {
            MatriculaId = matriculaId;
            AlunoId = alunoId;
            CursoId = cursoId;
            Valor = valor;
            MetodoPagamento = metodoPagamento;
            GatewayPagamento = gatewayPagamento;
            DataVencimento = dataVencimento;
            Status = "Pendente";
            
            ValidarPagamento();
        }

        public void DefinirDadosCartao(string numeroCartao, string nomeTitular, string validade, string cvv)
        {
            NumeroCartao = numeroCartao;
            NomeTitularCartao = nomeTitular;
            ValidadeCartao = validade;
            CvvCartao = cvv;
            AtualizarDataModificacao();
        }

        public void DefinirDadosPix(string chavePix, string qrCode)
        {
            ChavePix = chavePix;
            QrCodePix = qrCode;
            AtualizarDataModificacao();
        }

        public void DefinirDadosBoleto(string linhaDigitavel, string codigoBarras)
        {
            LinhaDigitavel = linhaDigitavel;
            CodigoBarras = codigoBarras;
            AtualizarDataModificacao();
        }

        public void DefinirTransacaoId(string transacaoId)
        {
            TransacaoId = transacaoId;
            AtualizarDataModificacao();
        }

        public void ConfirmarPagamento()
        {
            Status = "Confirmado";
            DataPagamento = DateTime.UtcNow;
            AtualizarDataModificacao();
        }

        public void RejeitarPagamento()
        {
            Status = "Rejeitado";
            AtualizarDataModificacao();
        }

        public void CancelarPagamento()
        {
            Status = "Cancelado";
            AtualizarDataModificacao();
        }

        public void ProcessarPagamento()
        {
            Status = "Processando";
            AtualizarDataModificacao();
        }

        public void ReembolsarPagamento()
        {
            Status = "Reembolsado";
            AtualizarDataModificacao();
        }

        public bool PodeSerCancelado()
        {
            return Status == "Pendente" || Status == "Processando";
        }

        public bool PodeSerReembolsado()
        {
            return Status == "Confirmado";
        }

        private void ValidarPagamento()
        {
            if (MatriculaId == Guid.Empty)
                throw new ArgumentException("Matrícula é obrigatória");

            if (AlunoId == Guid.Empty)
                throw new ArgumentException("Aluno é obrigatório");

            if (CursoId == Guid.Empty)
                throw new ArgumentException("Curso é obrigatório");

            if (Valor <= 0)
                throw new ArgumentException("Valor deve ser maior que zero");

            if (string.IsNullOrEmpty(MetodoPagamento))
                throw new ArgumentException("Método de pagamento é obrigatório");

            if (string.IsNullOrEmpty(GatewayPagamento))
                throw new ArgumentException("Gateway de pagamento é obrigatório");
        }
    }
} 