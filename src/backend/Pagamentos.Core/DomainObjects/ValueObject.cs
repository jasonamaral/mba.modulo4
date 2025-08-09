namespace Pagamentos.Core.DomainObjects
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;

            using (var thisComponents = GetEqualityComponents().GetEnumerator())
            using (var otherComponents = other.GetEqualityComponents().GetEnumerator())
            {
                while (thisComponents.MoveNext() && otherComponents.MoveNext())
                {
                    if (thisComponents.Current is null ^ otherComponents.Current is null)
                        return false;

                    if (thisComponents.Current != null && !thisComponents.Current.Equals(otherComponents.Current))
                        return false;
                }

                return !thisComponents.MoveNext() && !otherComponents.MoveNext();
            }
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) => HashCode.Combine(current, obj));
        }
    }
}
