using System;
using System.Collections.Generic;
using System.Linq;

namespace Temporal
{
    public static class TemporalTime
    {
        private static readonly ICollection<ITimeProvider> timeProviders = new List<ITimeProvider>();

        public static readonly IEnumerable<ITimeProvider> TimeProviders = timeProviders;

        public static void AddTimeProvider(ITimeProvider timeProvider)
        {
            timeProviders.Add(timeProvider);
        }

        public static void ClearTimeProviders()
        {
            timeProviders.Clear();
        }

        public static ITimeProvider CurrentTimeProvider
        {
            get
            {
                var timeProvider = TimeProviders.FirstOrDefault(x => x.Now.HasValue);

                return timeProvider ?? new SystemClockProvider();
            }
        }

        public static bool IsFrozen
        {
            get
            {
                if (CurrentTimeProvider == default(ITimeProvider))
                {
                    return false;
                }

                return CurrentTimeProvider.GetType().Name != nameof(SystemClockProvider);
            }
        }

        public static DateTime Now
        {
            get { return CurrentTimeProvider.Now.Value; }
        }

        public static DateTime UtcNow
        {
            get { return CurrentTimeProvider.UtcNow.Value; }
        }
    }
}
