using System;
using Xunit;
using Xunit.Sdk;

namespace Temporal.Tests
{
    [Collection("SystemClock")]
    public class SystemClockTests
    {
        public SystemClockTests()
        {
            SystemClock.Unfreeze();
        }

        [Fact]
        public void Now_ShouldInitiallyBeDateTimeNow()
        {
            var now = SystemClock.Now;

            Assert.Equal(DateTimeKind.Local, now.Kind);

            AssertCloseToCurrent(SystemClock.Now);
        }

        [Fact]
        public void UtcNow_ShouldInitiallyBeDateTimeUtcNow()
        {
            var utcNow = SystemClock.UtcNow;

            Assert.Equal(DateTimeKind.Utc, utcNow.Kind);

            AssertCloseToCurrent(SystemClock.UtcNow);
        }

        [Fact]
        public void IsFrozen_ShouldInitiallyBeFalse()
        {
            Assert.False(SystemClock.IsFrozen);
        }

        [Fact]
        public void Freeze_ShouldSetIsFrozenTrue()
        {
            SystemClock.Freeze(DateTime.Now);

            Assert.True(SystemClock.IsFrozen);
        }

        [Fact]
        public void Freeze_ShouldFreezeToExpectedDateTime()
        {
            var dateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);

            SystemClock.Freeze(dateTime);

            Assert.Equal(dateTime, SystemClock.Now);
        }

        [Fact]
        public void Freeze_ShouldFreezeToExpectedDateTimeWhenAlreadyFrozen()
        {
            SystemClock.Freeze(DateTime.Now);

            var dateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);

            SystemClock.Freeze(dateTime);

            Assert.Equal(dateTime, SystemClock.Now);
        }

        [Fact]
        public void Unfreeze_ShouldSetIsFrozenFalseWhenFrozen()
        {
            SystemClock.Freeze(DateTime.Now);

            SystemClock.Unfreeze();

            Assert.False(SystemClock.IsFrozen);
        }

        [Fact]
        public void Unfreeze_ShouldSetIsFrozenFalseWhenNotFrozen()
        {
            SystemClock.Unfreeze();

            Assert.False(SystemClock.IsFrozen);
        }

        [Fact]
        public void Unfreeze_ShouldSetDateTimeNowToCurrent()
        {
            SystemClock.Freeze(new DateTime(2000, 1, 1));

            SystemClock.Unfreeze();

            AssertCloseToCurrent(SystemClock.Now);
        }

        [Fact]
        public void Unfreeze_ShouldSetDateTimeUtcNowToCurrent()
        {
            SystemClock.Freeze(new DateTime(2000, 1, 1));

            SystemClock.Unfreeze();

            AssertCloseToCurrent(SystemClock.UtcNow);
        }

        private static void AssertCloseToCurrent(DateTime dateTime, int secondsRange = 60)
        {
            if (DateTime.UtcNow > dateTime.ToUniversalTime().AddSeconds(secondsRange / 2) ||
                DateTime.UtcNow < dateTime.ToUniversalTime().AddSeconds(-secondsRange / 2))
            {
                throw new XunitException(
                    $"dateTime of {dateTime} was not within {secondsRange} seconds of current time.");
            }
        }
    }
}
