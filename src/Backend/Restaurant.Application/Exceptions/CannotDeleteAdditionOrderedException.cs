namespace Restaurant.Application.Exceptions
{
    public sealed class CannotDeleteAdditionOrderedException : ApplicationException
    {
        public Guid AdditionId { get; }

        public CannotDeleteAdditionOrderedException(Guid additionId) : base($"Cannot delete Addition ordered with id:'{additionId}'")
        {
            AdditionId = additionId;
        }
    }
}
