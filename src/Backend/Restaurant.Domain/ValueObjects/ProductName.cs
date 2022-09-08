using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public sealed class ProductName
    {
        public string Value { get; }

        public ProductName(string productName)
        {
            ValidProductName(productName);
            Value = productName;
        }

        public static implicit operator string(ProductName productName)
            => productName.Value;

        public static implicit operator ProductName(string value)
            => new(value);

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
