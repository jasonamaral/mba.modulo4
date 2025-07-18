using System;

namespace Alunos.Domain.Common
{
    /// <summary>
    /// Classe base para todas as entidades do domínio
    /// </summary>
    public abstract class Entidade
    {
        /// <summary>
        /// Identificador único da entidade
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Data de criação da entidade
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Data da última atualização da entidade
        /// </summary>
        public DateTime UpdatedAt { get; protected set; }

        /// <summary>
        /// Construtor protegido para Entity Framework
        /// </summary>
        protected Entidade()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Construtor com ID específico
        /// </summary>
        /// <param name="id">ID da entidade</param>
        protected Entidade(Guid id)
        {
            Id = id;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Atualiza o timestamp de modificação
        /// </summary>
        protected void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Verifica se duas entidades são iguais
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is not Entidade other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return Id == other.Id;
        }

        /// <summary>
        /// Retorna o hash code da entidade
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Operador de igualdade
        /// </summary>
        public static bool operator ==(Entidade left, Entidade right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Operador de desigualdade
        /// </summary>
        public static bool operator !=(Entidade left, Entidade right)
        {
            return !Equals(left, right);
        }
    }
} 