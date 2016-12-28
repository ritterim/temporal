using System;

namespace Temporal
{
    public class SystemClockProvider : ITimeProvider
    {
        public DateTime? Now => DateTime.Now;
        public DateTime? UtcNow => DateTime.UtcNow;
        public bool SupportsFreeze => false;
        public bool IsFrozen => false;

        public void Freeze(DateTime dateTime)
        {
            throw new NotSupportedException(
                $"{typeof(SystemClockProvider).Name} does not support freeze.");
        }

        public void Unfreeze()
        {
            throw new NotSupportedException(
                $"{typeof(SystemClockProvider).Name} does not support unfreeze.");
        }
    }
}
