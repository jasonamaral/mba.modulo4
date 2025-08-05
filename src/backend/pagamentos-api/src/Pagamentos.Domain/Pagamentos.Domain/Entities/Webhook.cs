using Pagamentos.Domain.Common;

namespace Pagamentos.Domain.Entities
{
    public class Webhook : Entidade
    {
        public Guid? PagamentoId { get; private set; }
        public string Origem { get; private set; }
        public string Evento { get; private set; }
        public string Payload { get; private set; }
        public string Status { get; private set; }
        public DateTime DataRecebimento { get; private set; }
        public DateTime? DataProcessamento { get; private set; }
        public int TentativasProcessamento { get; private set; }
        public string ErroProcessamento { get; private set; }

        private Webhook() : base()
        {
        }

        public Webhook(
            string origem,
            string evento,
            string payload,
            Guid? pagamentoId = null) : base()
        {
            Origem = origem;
            Evento = evento;
            Payload = payload;
            PagamentoId = pagamentoId;
            Status = "Recebido";
            DataRecebimento = DateTime.UtcNow;
            TentativasProcessamento = 0;

            ValidarWebhook();
        }

        public void ProcessarWebhook()
        {
            Status = "Processando";
            TentativasProcessamento++;
            AtualizarDataModificacao();
        }

        public void CompletarProcessamento()
        {
            Status = "Processado";
            DataProcessamento = DateTime.UtcNow;
            AtualizarDataModificacao();
        }

        public void FalharProcessamento(string erro)
        {
            Status = "Falha";
            ErroProcessamento = erro;
            AtualizarDataModificacao();
        }

        public void DefinirPagamentoId(Guid pagamentoId)
        {
            PagamentoId = pagamentoId;
            AtualizarDataModificacao();
        }

        public bool PodeSerReprocessado()
        {
            return Status == "Falha" && TentativasProcessamento < 5;
        }

        private void ValidarWebhook()
        {
            if (string.IsNullOrEmpty(Origem))
                throw new ArgumentException("Origem é obrigatória");

            if (string.IsNullOrEmpty(Evento))
                throw new ArgumentException("Evento é obrigatório");

            if (string.IsNullOrEmpty(Payload))
                throw new ArgumentException("Payload é obrigatório");
        }
    }
} 