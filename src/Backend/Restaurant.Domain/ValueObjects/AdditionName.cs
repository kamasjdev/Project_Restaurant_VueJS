using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public class AdditionName : IEquatable<AdditionName>
    {
        private string _value;
        public virtual string Value { get { return _value; } protected set { _value = value; } }

        protected AdditionName() { }

        public AdditionName(string addtionName)
        {
            ValidAdditionName(addtionName);
            _value = addtionName;
        }

        public static implicit operator string(AdditionName additionName)
            => additionName.Value;

        public static implicit operator AdditionName(string value)
            => new(value);

        public override bool Equals(object obj)
        {
            return Equals(obj as AdditionName);
        }

        public bool Equals(AdditionName other)
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

        private static void ValidAdditionName(string additionName)
        {
            if (string.IsNullOrWhiteSpace(additionName))
            {
                throw new AdditionNameCannotBeEmptyException();
            }

            if (additionName.Length < 3)
            {
                throw new AdditionNameTooShortException(additionName);
            }
        }
    }
}
