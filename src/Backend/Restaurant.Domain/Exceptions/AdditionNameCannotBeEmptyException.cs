namespace Restaurant.Domain.Exceptions
{
    public sealed class AdditionNameCannotBeEmptyException : DomainException
    {
        public AdditionNameCannotBeEmptyException() : base("AdditionName cannot be empty")
        {
        }
    }
}
