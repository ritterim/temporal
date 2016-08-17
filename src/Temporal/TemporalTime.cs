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

        public static bool IsFrozen
        {
            get
            {
                var firstTimeProviderWithValue = TimeProviders.FirstOrDefault(x => x.Now.HasValue);

                if (firstTimeProviderWithValue == default(ITimeProvider))
                {
                    return false;
                }

                return firstTimeProviderWithValue.GetType().Name != nameof(SystemClockProvider);
            }
        }

        public static DateTime Now
        {
            get
            {
                var timeProvider = TimeProviders.FirstOrDefault(x => x.Now.HasValue);

                if (timeProvider == null)
                {
                    throw new ApplicationException("No time provider is ready to return the time.");
                }

                return timeProvider.Now.Value;
            }
        }

        public static DateTime UtcNow
        {
            get
            {
                var timeProvider = TimeProviders.FirstOrDefault(x => x.UtcNow.HasValue);

                if (timeProvider == null)
                {
                    throw new ApplicationException("No time provider is ready to return the time.");
                }

                return timeProvider.UtcNow.Value;
            }
        }
    }
}
