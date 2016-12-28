using System;

namespace Temporal
{
    public interface ITimeProvider
    {
        DateTime? Now { get; }
        DateTime? UtcNow { get; }
        bool SupportsFreeze { get; }
        bool IsFrozen { get; }
        void Freeze(DateTime dateTime);
        void Unfreeze();
    }
}
