using System;

namespace Temporal.Tests
{
    public class TestFreezeableTimeProvider : ITimeProvider
    {
        private DateTime? _frozenValue;

        public DateTime? Now => _frozenValue.HasValue ? _frozenValue.Value.ToLocalTime() : (DateTime?)null;

        public bool SupportsFreeze => true;

        public bool IsFrozen => _frozenValue.HasValue;

        public DateTime? UtcNow => _frozenValue.HasValue ? _frozenValue.Value.ToUniversalTime() : (DateTime?)null;

        public void Freeze(DateTime dateTime)
        {
            _frozenValue = dateTime;
        }

        public void Unfreeze()
        {
            _frozenValue = null;
        }
    }
}
