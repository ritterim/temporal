using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace Temporal.Tests
{
    public class IAppBuilderExtensionsTests
    {
        [Fact(Skip = "An issue with context being unavailable in CookieService GetValue is making this test difficult.")]
        public async Task CurrentInfoUri_ShouldReturnExpectedResponse()
        {
            var freezeDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var options = new TemporalOptions();

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var response = await server.CreateRequest(options.CurrentInfoUri)
                    .AddHeader("cookie", freezeDateTime.ToString("o"))
                    .GetAsync();

                response.EnsureSuccessStatusCode();

                var responseType = new
                {
                    Now = new DateTime(),
                    UtcNow = new DateTime()
                };

                var responseObj = JsonConvert.DeserializeAnonymousType(
                    await response.Content.ReadAsStringAsync(),
                    responseType);

                Assert.Equal(freezeDateTime, responseObj.UtcNow);
                Assert.Equal(freezeDateTime.ToLocalTime(), responseObj.Now);
            }
        }

        [Fact]
        public async Task FreezeUri_ShouldSetCookie()
        {
            var freezeDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var options = new TemporalOptions();

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "utc", freezeDateTime.ToString("o") }
                });
                var response = await server.HttpClient.PostAsync(options.FreezeUri, content);

                response.EnsureSuccessStatusCode();

                var setCookieHeaderValue = response.Headers
                    .Single(x => x.Key == "Set-Cookie").Value.Single();
                var expectedStartsWith = CookieTimeProvider.CookieName +
                    $"={HttpUtility.UrlEncode(freezeDateTime.ToString("o")).ToUpper()}";

                Assert.True(setCookieHeaderValue.StartsWith(expectedStartsWith),
                    $"Expected\n{setCookieHeaderValue}\nto start with\n{expectedStartsWith}");
            }
        }

        [Fact]
        public async Task FreezeUri_ShouldReturnBadRequestWhenMissingQueryStringParameter()
        {
            var options = new TemporalOptions();

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
            var options = new TemporalOptions();

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
        public async Task UnfreezeUri_ShouldRemoveCookie()
        {
            var options = new TemporalOptions();

            using (var server = TestServer.Create(app =>
            {
                app.UseTemporal(options);
            }))
            {
                var content = new StringContent("");
                var response = await server.HttpClient.PostAsync(options.UnfreezeUri, content);

                response.EnsureSuccessStatusCode();

                var setCookieHeaderValue = response.Headers
                    .Single(x => x.Key == "Set-Cookie").Value.Single();

                Assert.Equal(
                    $"{CookieTimeProvider.CookieName}=; path=/; expires=Thu, 01-Jan-1970 00:00:00 GMT",
                    setCookieHeaderValue);
            }
        }

        [Fact]
        public async Task UnfreezeUri_ShouldReturnMethodNotAllowedForGet()
        {
            var options = new TemporalOptions();

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
