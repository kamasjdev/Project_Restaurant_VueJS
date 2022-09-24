using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public class ProductName : IEquatable<ProductName>
    {
        private string _value;
        public virtual string Value { get { return _value; } protected set { _value = value; } }

        protected ProductName() { }

        public ProductName(string productName)
        {
            ValidProductName(productName);
            _value = productName;
        }

        public static implicit operator string(ProductName productName)
            => productName.Value;

        public static implicit operator ProductName(string value)
            => new(value);

        public override bool Equals(object obj)
        {
            return Equals(obj as ProductName);
        }

        public bool Equals(ProductName other)
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

        private static void ValidProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ProductNameCannotBeEmptyException();
            }

            if (productName.Length < 3)
            {
                throw new ProductNameTooShortException(productName);
            }
        }
    }
}
