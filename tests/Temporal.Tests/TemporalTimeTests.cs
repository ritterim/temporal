using System;
using System.Linq;
using Xunit;

namespace Temporal.Tests
{
    [Collection("TemporalTime")]
    public class TemporalTimeTests
    {
        public TemporalTimeTests()
        {
            TemporalTime.ClearTimeProviders();
        }

        [Fact]
        public void ShouldInitiallyHaveNoTimeProviders()
        {
            Assert.False(TemporalTime.TimeProviders.Any());
        }

        [Fact]
        public void AddTimeProvider_ShouldAddExpectedTimeProvider()
        {
            TemporalTime.AddTimeProvider(new TestTimeProvider());

            Assert.Equal(TestTimeProvider.DefaultNow, TemporalTime.TimeProviders.Single().Now);
        }

        [Fact]
        public void AddTimeProvider_ShouldPermitTwoProvidersOfTheSameType()
        {
            TemporalTime.AddTimeProvider(new TestTimeProvider());
            TemporalTime.AddTimeProvider(new TestTimeProvider());

            Assert.Equal(2, TemporalTime.TimeProviders.Count());
        }

        [Fact]
        public void ClearTimeProviders_ShouldClearAllTimeProviders()
        {
            TemporalTime.AddTimeProvider(new TestTimeProvider());

            Assert.Equal(1, TemporalTime.TimeProviders.Count());

            TemporalTime.ClearTimeProviders();

            Assert.False(TemporalTime.TimeProviders.Any());
        }

        [Fact]
        public void CurrentTimeProvider_UsesSystemClockProviderWhenNoTimeProvidersHaveTheTime()
        {
            TemporalTime.AddTimeProvider(new NoTimeTestTimeProvider());

            Assert.IsType<SystemClockProvider>(TemporalTime.CurrentTimeProvider);
        }

        [Fact]
        public void CurrentTimeProvider_ReturnsTheTimeFromTheFirstTimeProviderWithTheTime()
        {
            TemporalTime.AddTimeProvider(new NoTimeTestTimeProvider());
            TemporalTime.AddTimeProvider(new TestTimeProvider());

            Assert.IsType<TestTimeProvider>(TemporalTime.CurrentTimeProvider);
        }

        [Fact]
        public void IsFrozen_ReturnsFalseWhenSystemClockProviderReturnsValue()
        {
            TemporalTime.AddTimeProvider(new SystemClockProvider());

            Assert.False(TemporalTime.IsFrozen);
        }

        [Fact]
        public void IsFrozen_ReturnsFalseWhenCookieProviderReturnsNull()
        {
            var cookieTimeProvider = new CookieTimeProvider(new TestCookieService(null));
            TemporalTime.AddTimeProvider(cookieTimeProvider);

            Assert.False(TemporalTime.IsFrozen);
        }

        [Fact]
        public void IsFrozen_ReturnsTrueWhenCookieProviderReturnsValue()
        {
            var cookieTimeProvider = new CookieTimeProvider(new TestCookieService(DateTime.Now.ToString("o")));
            TemporalTime.AddTimeProvider(cookieTimeProvider);

            Assert.True(TemporalTime.IsFrozen);
        }

        [Fact]
        public void Now_ReturnsTheTimeFromTheFirstTimeProviderWithTheTime()
        {
            TemporalTime.AddTimeProvider(new TestTimeProvider());

            Assert.Equal(TestTimeProvider.DefaultNow, TemporalTime.Now);
        }

        [Fact]
        public void UtcNow_ReturnsTheTimeFromTheFirstTimeProviderWithTheTime()
        {
            TemporalTime.AddTimeProvider(new TestTimeProvider());

            Assert.Equal(TestTimeProvider.DefaultUtcNow, TemporalTime.UtcNow);
        }

        private class TestTimeProvider : ITimeProvider
        {
            public static readonly DateTime DefaultNow = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);
            public static readonly DateTime DefaultUtcNow = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            public TestTimeProvider(
                DateTime? now = null,
                DateTime? utcNow = null)
            {
                Now = now ?? DefaultNow;
                UtcNow = utcNow ?? DefaultUtcNow;
            }

            public DateTime? Now { get; set; }

            public DateTime? UtcNow { get; set; }

            public bool SupportsFreeze
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsFrozen
            {
                get { throw new NotImplementedException(); }
            }

            public void Freeze(DateTime dateTime)
            {
                throw new NotImplementedException();
            }

            public void Unfreeze()
            {
                throw new NotImplementedException();
            }
        }

        private class NoTimeTestTimeProvider : ITimeProvider
        {
            public DateTime? Now => null;
            public DateTime? UtcNow => null;

            public bool SupportsFreeze
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsFrozen
            {
                get { throw new NotImplementedException(); }
            }

            public void Freeze(DateTime dateTime)
            {
                throw new NotImplementedException();
            }

            public void Unfreeze()
            {
                throw new NotImplementedException();
            }
        }
    }
}
