using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public class Price : IEquatable<Price>
    {
        private decimal _value;
        public virtual decimal Value { get { return _value; } protected set { _value = value; } }

        protected Price() { }

        public Price(decimal price)
        {
            ValidPrice(price);
            _value = price;
        }

        public static implicit operator decimal(Price additionName)
            => additionName.Value;

        public static implicit operator Price(decimal value)
            => new(value);

        public override bool Equals(object obj)
        {
            return Equals(obj as Price);
        }

        public bool Equals(Price other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                    .Select(x => x != null ? x.GetHashCode() : 0)
                    .Aggregate((x, y) => x ^ y);
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private static void ValidPrice(decimal price)
        {
            if (price < 0)
            {
                throw new PriceCannotBeNegativeException(price);
            }
        }
    }
}
