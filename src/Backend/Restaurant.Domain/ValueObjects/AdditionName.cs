using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.ValueObjects
{
    public class AdditionName
    {
        public string Value { get; }

        protected AdditionName() { }

        public AdditionName(string addtionName)
        {
            ValidAdditionName(addtionName);
            Value = addtionName;
        }

        public static implicit operator string(AdditionName additionName)
            => additionName.Value;

        public static implicit operator AdditionName(string value)
            => new(value);

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
