namespace Restaurant.Domain.Exceptions
{
    public sealed class AdditionNameTooShortException : DomainException
    {
        public string AddtionName { get; }

        public AdditionNameTooShortException(string addtionName) : base($"AdditionName: '{addtionName}' should have at least 3 characters")
        {
            AddtionName = addtionName;
        }
    }
}
