namespace Restaurant.Application.Exceptions
{
    public sealed class AdditionNotFoundException : ApplicationException
    {
        public Guid AdditionId { get; }

        public AdditionNotFoundException(Guid additionId) : base($"Addition with id: '{additionId}' was not found")
        {
            AdditionId = additionId;
        }
    }
}
