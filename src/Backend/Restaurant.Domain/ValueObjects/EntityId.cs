using Restaurant.Domain.Exceptions;

namespace Restaurant.Domain.Entities
{
    public class EntityId
    {
        public Guid Value { get; }

        public EntityId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new InvalidEntityIdException(value);
            }

            Value = value;
        }

        public static EntityId Create() => new(Guid.NewGuid());

        public static implicit operator Guid(EntityId date)
            => date.Value;

        public static implicit operator EntityId(Guid value)
            => new(value);
    }
}
