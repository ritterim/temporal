using System;

namespace Temporal
{
    public class SystemClockProvider : ITimeProvider
    {
        public DateTime? Now => DateTime.Now;
        public DateTime? UtcNow => DateTime.UtcNow;
    }
}
