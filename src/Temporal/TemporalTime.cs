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
                return TimeProviders.FirstOrDefault(x => x.Now.HasValue);
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
            get
            {
                if (CurrentTimeProvider == null)
                {
                    throw new ApplicationException("No time provider is ready to return the time.");
                }

                return CurrentTimeProvider.Now.Value;
            }
        }

        public static DateTime UtcNow
        {
            get
            {
                if (CurrentTimeProvider == null)
                {
                    throw new ApplicationException("No time provider is ready to return the time.");
                }

                return CurrentTimeProvider.UtcNow.Value;
            }
        }
    }
}
