using Newtonsoft.Json;
using Owin;
using System;
using System.Threading.Tasks;

namespace Temporal
{
    public static class IAppBuilderExtensions
    {
        public static void UseTemporal(this IAppBuilder app, TemporalOptions options)
        {
            TemporalTime.AddTimeProvider(new CookieTimeProvider(new CookieService()));
            TemporalTime.AddTimeProvider(new SystemClockProvider());

            app.Map(options.CurrentInfoUri, getDateTime =>
            {
                getDateTime.Use(async (context, next) =>
                {
                    var response = new
                    {
                        Now = TemporalTime.Now,
                        UtcNow = TemporalTime.UtcNow
                    };

                    var json = JsonConvert.SerializeObject(response);

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                });
            });

            app.Map(options.FreezeUri, freeze =>
            {
                freeze.Use(async (context, next) =>
                {
                    var query = context.Request.Query["utc"];

                    DateTime dateTime;
                    if (DateTime.TryParse(query, out dateTime))
                    {
                        dateTime = dateTime.ToUniversalTime();

                        var cookieTimeProvider = new CookieTimeProvider(new CookieService(() => context));
                        cookieTimeProvider.SetCookie(dateTime);

                        context.Response.StatusCode = 200;
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }

                    await Task.FromResult(true);
                });
            });

            app.Map(options.UnfreezeUri, unfreeze =>
            {
                unfreeze.Use(async (context, next) =>
                {
                    var cookieTimeProvider = new CookieTimeProvider(new CookieService(() => context));
                    cookieTimeProvider.RemoveCookie();

                    context.Response.StatusCode = 200;

                    await Task.FromResult(true);
                });
            });
        }
    }
}
