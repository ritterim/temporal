using System;
using System.Collections.Generic;

namespace Temporal
{
    public class TemporalOptions
    {
        private readonly string _temporalRootPath;

        public TemporalOptions(string temporalRootPath = TemporalConstants.TemporalOptions.DefaultTemporalRootPath)
        {
            _temporalRootPath = temporalRootPath;

            if (!_temporalRootPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                _temporalRootPath += "/";
        }

        public string CurrentInfoUri => _temporalRootPath + TemporalConstants.TemporalOptions.CurrentInfoEndpoint;
        public string FreezeUri => _temporalRootPath + TemporalConstants.TemporalOptions.FreezeEndpoint;
        public string UnfreezeUri => _temporalRootPath + TemporalConstants.TemporalOptions.UnfreezeEndpoint;

        /// <summary>
        /// The current alignment of the on-screen time machine.
        /// </summary>
        public TimeMachineAlignment TimeMachineAlignment { get; private set; }

        /// <summary>
        /// The time providers to be added when initialized.
        /// </summary>
        public ICollection<ITimeProvider> TimeProviders { get; private set; } = new List<ITimeProvider>();

        /// <summary>
        /// The current setting if the application should reload the page
        /// when toggling between frozen and live time.
        /// </summary>
        public bool AutoRefresh { get; private set; } = true;

        /// <summary>
        /// Add an ITimeProvider instance to this TemporalOptions instance.
        /// </summary>
        public TemporalOptions AddTimeProvider(ITimeProvider timeProvider)
        {
            TimeProviders.Add(timeProvider);

            return this;
        }

        /// <summary>
        /// Set the alignment of the on-screen time machine.
        /// </summary>
        public TemporalOptions SetTimeMachineAlignment(TimeMachineAlignment alignment)
        {
            TimeMachineAlignment = alignment;

            return this;
        }

        /// <summary>
        /// Set whether the application should reload the page
        /// when toggling between frozen and live time.
        /// Defaults to true.
        /// </summary>
        public TemporalOptions SetAutoRefresh(bool autoRefresh)
        {
            AutoRefresh = autoRefresh;

            return this;
        }
    }
}
