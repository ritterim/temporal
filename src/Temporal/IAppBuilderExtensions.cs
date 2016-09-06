using Newtonsoft.Json;
using Owin;
using System;
using System.Net.Http;
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
                    if (context.Request.Method != HttpMethod.Get.Method)
                    {
                        context.Response.StatusCode = 405;
                        return;
                    }

                    var response = new
                    {
                        TimeProvider = TemporalTime.CurrentTimeProvider.GetType().Name,
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
                    if (context.Request.Method != HttpMethod.Post.Method)
                    {
                        context.Response.StatusCode = 405;
                        return;
                    }

                    var form = await context.Request.ReadFormAsync();

                    DateTime dateTime;
                    if (DateTime.TryParse(form.Get("utc"), out dateTime))
                    {
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
                    if (context.Request.Method != HttpMethod.Post.Method)
                    {
                        context.Response.StatusCode = 405;
                        return;
                    }

                    var cookieTimeProvider = new CookieTimeProvider(new CookieService(() => context));
                    cookieTimeProvider.RemoveCookie();

                    context.Response.StatusCode = 200;

                    await Task.FromResult(true);
                });
            });
        }
    }
}
