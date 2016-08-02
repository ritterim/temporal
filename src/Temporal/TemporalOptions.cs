using System;

namespace Temporal
{
    public class TemporalOptions
    {
        private readonly string _temporalRootPath;

        public TemporalOptions(string temporalRootPath = Constants.TemporalOptions.DefaultTemporalRootPath)
        {
            _temporalRootPath = temporalRootPath;

            if (!_temporalRootPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                _temporalRootPath += "/";
        }

        public string CurrentInfoUri => _temporalRootPath + Constants.TemporalOptions.CurrentInfoEndpoint;
        public string FreezeUri => _temporalRootPath + Constants.TemporalOptions.FreezeEndpoint;
        public string UnfreezeUri => _temporalRootPath + Constants.TemporalOptions.UnfreezeEndpoint;

        /// <summary>
        /// The current alignment of the on-screen time machine.
        /// </summary>
        public TimeMachineAlignment TimeMachineAlignment { get; private set; }

        /// <summary>
        /// Set the alignment of the on-screen time machine.
        /// </summary>
        public TemporalOptions SetTimeMachineAlignment(TimeMachineAlignment alignment)
        {
            TimeMachineAlignment = alignment;

            return this;
        }
    }
}
