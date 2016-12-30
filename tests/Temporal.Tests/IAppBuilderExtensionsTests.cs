using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Temporal.Tests
{
    public class IAppBuilderExtensionsTests
    {
        [Fact]
        public async Task CurrentInfoUri_ShouldReturnExpectedResponse()
        {
            var freezeDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var timeProvider = new TestFreezeableTimeProvider();
            timeProvider.Freeze(freezeDateTime);

            var options = new TemporalOptions()
                .AddTimeProvider(timeProvider);

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var response = await server.HttpClient.GetAsync(options.CurrentInfoUri);

                response.EnsureSuccessStatusCode();

                var responseType = new
                {
                    TimeProvider = "",
                    Now = new DateTime(),
                    UtcNow = new DateTime()
                };

                var responseObj = JsonConvert.DeserializeAnonymousType(
                    await response.Content.ReadAsStringAsync(),
                    responseType);

                Assert.Equal("TestFreezeableTimeProvider", responseObj.TimeProvider);
                Assert.Equal(freezeDateTime, responseObj.UtcNow);
                Assert.Equal(freezeDateTime.ToLocalTime(), responseObj.Now);
            }
        }

        [Fact]
        public async Task FreezeUri_ShouldFreeze()
        {
            var freezeUtc = "2000-01-01T00:00:00";

            var options = new TemporalOptions()
                .AddTimeProvider(new TestFreezeableTimeProvider());

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "utc", freezeUtc }
                });
                var response = await server.HttpClient.PostAsync(options.FreezeUri, content);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task FreezeUri_ShouldReturnBadRequestWhenMissingQueryStringParameter()
        {
            var options = new TemporalOptions()
                .AddTimeProvider(new SystemClockProvider());

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var content = new StringContent("");
                var response = await server.HttpClient.PostAsync(options.FreezeUri, content);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task FreezeUri_ShouldReturnMethodNotAllowedForGet()
        {
            var options = new TemporalOptions()
                .AddTimeProvider(new SystemClockProvider());

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var response = await server.HttpClient.GetAsync(options.FreezeUri);

                Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            }
        }

        [Fact]
        public async Task UnfreezeUri_ShouldUnfreezeIfFrozen()
        {
            var timeProvider = new TestFreezeableTimeProvider();
            timeProvider.Freeze(DateTime.UtcNow);

            var options = new TemporalOptions()
                .AddTimeProvider(timeProvider);

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var content = new StringContent("");
                var response = await server.HttpClient.PostAsync(options.UnfreezeUri, content);

                response.EnsureSuccessStatusCode();

                Assert.Null(timeProvider.Now);
                Assert.Null(timeProvider.UtcNow);
            }
        }

        [Fact]
        public async Task UnfreezeUri_ShouldReturnMethodNotAllowedForGet()
        {
            var options = new TemporalOptions()
                .AddTimeProvider(new SystemClockProvider());

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var response = await server.HttpClient.GetAsync(options.UnfreezeUri);

                Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            }
        }
    }
}
