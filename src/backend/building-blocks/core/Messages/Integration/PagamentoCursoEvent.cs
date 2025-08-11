namespace Core.Messages.Integration
{
    public class PagamentoCursoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public Guid AlunoId { get; private set; }
        public decimal Total { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public PagamentoCursoEvent(Guid pedidoId,
                                     Guid clienteId,
                                     decimal total,
                                     string nomeCartao,
                                     string numeroCartao,
                                     string expiracaoCartao,
                                     string cvvCartao)
        {
            //AggregateID = pedidoId;
            PedidoId = pedidoId;
            AlunoId = clienteId;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }
    }
}
