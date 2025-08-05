namespace Alunos.Domain.Common;

public abstract class Entidade
{
    #region Atributos
    public Guid Id { get; protected set; }

    public DateTime DataCriacao { get; protected set; }

    public DateTime DataAlteracao { get; protected set; }
    #endregion

    #region CTOR
    protected Entidade()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
    }

    protected Entidade(Guid id)
    {
        Id = id;
        DataCriacao = DateTime.UtcNow;
    }
    #endregion

    #region Métodos
    public void RegistrarDataAlteracao()
    {
        DataAlteracao = DateTime.UtcNow;
    }

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

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    public static bool operator ==(Entidade left, Entidade right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entidade left, Entidade right)
    {
        return !Equals(left, right);
    }
    #endregion
}