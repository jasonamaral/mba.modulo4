using Pagamentos.Domain.Common;

namespace Pagamentos.Domain.Entities
{
    public class Transacao : Entidade
    {
        public Guid PagamentoId { get; private set; }
        public string TipoTransacao { get; private set; } // Cobranca, Captura, Cancelamento, Reembolso
        public decimal Valor { get; private set; }
        public string Status { get; private set; }
        public string ReferenciaTid { get; private set; }
        public string AutorizacaoId { get; private set; }
        public string ResponseJson { get; private set; }
        public DateTime DataTransacao { get; private set; }

        private Transacao() : base()
        {
        }

        public Transacao(
            Guid pagamentoId,
            string tipoTransacao,
            decimal valor,
            string status,
            string referenciaTid = null,
            string autorizacaoId = null,
            string responseJson = null) : base()
        {
            PagamentoId = pagamentoId;
            TipoTransacao = tipoTransacao;
            Valor = valor;
            Status = status;
            ReferenciaTid = referenciaTid;
            AutorizacaoId = autorizacaoId;
            ResponseJson = responseJson;
            DataTransacao = DateTime.UtcNow;

            ValidarTransacao();
        }

        public void AtualizarStatus(string novoStatus)
        {
            Status = novoStatus;
            AtualizarDataModificacao();
        }

        public void DefinirReferenciaTid(string referenciaTid)
        {
            ReferenciaTid = referenciaTid;
            AtualizarDataModificacao();
        }

        public void DefinirAutorizacaoId(string autorizacaoId)
        {
            AutorizacaoId = autorizacaoId;
            AtualizarDataModificacao();
        }

        public void DefinirResponseJson(string responseJson)
        {
            ResponseJson = responseJson;
            AtualizarDataModificacao();
        }

        private void ValidarTransacao()
        {
            if (PagamentoId == Guid.Empty)
                throw new ArgumentException("PagamentoId é obrigatório");

            if (string.IsNullOrEmpty(TipoTransacao))
                throw new ArgumentException("Tipo de transação é obrigatório");

            if (Valor <= 0)
                throw new ArgumentException("Valor deve ser maior que zero");

            if (string.IsNullOrEmpty(Status))
                throw new ArgumentException("Status é obrigatório");
        }
    }
} 