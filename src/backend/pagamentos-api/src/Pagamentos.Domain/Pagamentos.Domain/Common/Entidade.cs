using System;

namespace Pagamentos.Domain.Common
{
    public abstract class Entidade
    {
        public Guid Id { get; protected set; }
        public DateTime CriadoEm { get; protected set; }
        public DateTime AtualizadoEm { get; protected set; }

        protected Entidade()
        {
            Id = Guid.NewGuid();
            CriadoEm = DateTime.UtcNow;
            AtualizadoEm = DateTime.UtcNow;
        }

        protected Entidade(Guid id)
        {
            Id = id;
            CriadoEm = DateTime.UtcNow;
            AtualizadoEm = DateTime.UtcNow;
        }

        public void AtualizarDataModificacao()
        {
            AtualizadoEm = DateTime.UtcNow;
        }

        // Método público para permitir que a infraestrutura defina as datas
        public void DefinirCriadoEm(DateTime criadoEm)
        {
            CriadoEm = criadoEm;
        }

        public void DefinirAtualizadoEm(DateTime atualizadoEm)
        {
            AtualizadoEm = atualizadoEm;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Entidade)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entidade left, Entidade right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entidade left, Entidade right)
        {
            return !Equals(left, right);
        }
    }
} 