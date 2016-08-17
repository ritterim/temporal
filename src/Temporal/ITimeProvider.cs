using System;

namespace Temporal
{
    public interface ITimeProvider
    {
        DateTime? Now { get; }
        DateTime? UtcNow { get; }
    }
}
