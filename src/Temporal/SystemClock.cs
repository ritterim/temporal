using System;

namespace Temporal
{
    public static class SystemClock
    {
        private static DateTimeOffset? frozenAt { get; set; }

        public static DateTime Now => frozenAt.HasValue
            ? frozenAt.Value.LocalDateTime
            : DateTime.Now;

        public static DateTime UtcNow => frozenAt.HasValue
            ? frozenAt.Value.UtcDateTime
            : DateTime.UtcNow;

        public static bool IsFrozen => frozenAt.HasValue;

        public static void Freeze(DateTime newSystemTime)
        {
            frozenAt = newSystemTime;
        }

        public static void Freeze(DateTimeOffset newSystemTime)
        {
            frozenAt = newSystemTime.DateTime;
        }

        public static void Unfreeze()
        {
            frozenAt = null;
        }
    }
}
