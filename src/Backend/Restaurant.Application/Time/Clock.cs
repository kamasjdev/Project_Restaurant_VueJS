using Restaurant.Application.Abstractions;

namespace Restaurant.Application.Time
{
    internal sealed class Clock : IClock
    {
        public DateTime CurrentDate()
            => DateTime.UtcNow;
    }
}
