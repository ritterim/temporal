using System;

namespace Temporal
{
    public static class SystemClock
    {
        private static DateTime? frozenDateTime { get; set; }

        public static DateTime Now => frozenDateTime.HasValue
            ? frozenDateTime.Value.ToLocalTime()
            : DateTime.Now;

        public static DateTime UtcNow => frozenDateTime.HasValue
            ? frozenDateTime.Value.ToUniversalTime()
            : DateTime.UtcNow;

        public static bool IsFrozen => frozenDateTime.HasValue;

        public static void Freeze(DateTime newSystemTime)
        {
            frozenDateTime = newSystemTime;
        }

        public static void Unfreeze()
        {
            frozenDateTime = null;
        }
    }
}
