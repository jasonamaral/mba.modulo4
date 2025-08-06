using Pagamentos.Domain.Common;

namespace Pagamentos.Domain.Entities
{
    public class Reembolso : Entidade
    {
        public Guid PagamentoId { get; private set; }
        public decimal Valor { get; private set; }
        public string Status { get; private set; }
        public string Motivo { get; private set; }
        public string ReembolsoId { get; private set; }
        public DateTime DataSolicitacao { get; private set; }
        public DateTime? DataProcessamento { get; private set; }
        public string MotivoRejeicao { get; private set; }

        private Reembolso() : base()
        {
        }

        public Reembolso(
            Guid pagamentoId,
            decimal valor,
            string motivo) : base()
        {
            PagamentoId = pagamentoId;
            Valor = valor;
            Motivo = motivo;
            Status = "Solicitado";
            DataSolicitacao = DateTime.UtcNow;

            ValidarReembolso();
        }

        public void AprovarReembolso(string reembolsoId)
        {
            Status = "Aprovado";
            ReembolsoId = reembolsoId;
            AtualizarDataModificacao();
        }

        public void ProcessarReembolso()
        {
            Status = "Processado";
            DataProcessamento = DateTime.UtcNow;
            AtualizarDataModificacao();
        }

        public void RejeitarReembolso(string motivoRejeicao)
        {
            Status = "Rejeitado";
            MotivoRejeicao = motivoRejeicao;
            AtualizarDataModificacao();
        }

        public void CancelarReembolso()
        {
            Status = "Cancelado";
            AtualizarDataModificacao();
        }

        public bool PodeSerProcessado()
        {
            return Status == "Aprovado";
        }

        public bool PodeSerCancelado()
        {
            return Status == "Solicitado" || Status == "Aprovado";
        }

        private void ValidarReembolso()
        {
            if (PagamentoId == Guid.Empty)
                throw new ArgumentException("PagamentoId é obrigatório");

            if (Valor <= 0)
                throw new ArgumentException("Valor deve ser maior que zero");

            if (string.IsNullOrEmpty(Motivo))
                throw new ArgumentException("Motivo é obrigatório");
        }
    }
} 