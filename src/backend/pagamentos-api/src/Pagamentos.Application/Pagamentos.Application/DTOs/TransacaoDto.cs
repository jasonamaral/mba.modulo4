using System;

namespace Pagamentos.Application.DTOs
{
    public class TransacaoDto
    {
        public Guid Id { get; set; }
        public Guid PagamentoId { get; set; }
        public string TipoTransacao { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public string ReferenciaTid { get; set; }
        public string AutorizacaoId { get; set; }
        public DateTime DataTransacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
} 