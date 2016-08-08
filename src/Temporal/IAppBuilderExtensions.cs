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
            app.Map(options.CurrentInfoUri, getDateTime =>
            {
                getDateTime.Use(async (context, next) =>
                {
                    var response = new
                    {
                        Now = SystemClock.Now,
                        UtcNow = SystemClock.UtcNow,
                        IsFrozen = SystemClock.IsFrozen
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
                        SystemClock.Freeze(dateTime);

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
                    SystemClock.Unfreeze();

                    context.Response.StatusCode = 200;

                    await Task.FromResult(true);
                });
            });
        }
    }
}
