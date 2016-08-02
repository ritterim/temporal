using System;

namespace Temporal
{
    public static class SystemClock
    {
        private static DateTimeOffset? frozenDateTime { get; set; }

        public static DateTime Now => frozenDateTime.HasValue
            ? frozenDateTime.Value.LocalDateTime
            : DateTime.Now;

        public static DateTime UtcNow => frozenDateTime.HasValue
            ? frozenDateTime.Value.UtcDateTime
            : DateTime.UtcNow;

        public static bool IsFrozen => frozenDateTime.HasValue;

        public static void Freeze(DateTime newSystemTime)
        {
            frozenDateTime = newSystemTime;
        }

        public static void Freeze(DateTimeOffset newSystemTime)
        {
            frozenDateTime = newSystemTime.DateTime;
        }

        public static void Unfreeze()
        {
            frozenDateTime = null;
        }
    }
}
