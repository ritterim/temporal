using Newtonsoft.Json;
using Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Temporal
{
    public static class IAppBuilderExtensions
    {
        public static void UseTemporal(this IAppBuilder app, TemporalOptions options)
        {
            if (!options.TimeProviders.Any())
            {
                throw new ArgumentException(
                    $"{nameof(options.TimeProviders)} must contain at least 1 time provider.");
            }

            TemporalTime.ClearTimeProviders();

            foreach (var timeProvider in options.TimeProviders)
            {
                TemporalTime.AddTimeProvider(timeProvider);
            }

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
                        var firstFreezableTimeProvider = TemporalTime.TimeProviders.FirstOrDefault(x => x.SupportsFreeze);

                        if (firstFreezableTimeProvider == null)
                        {
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync(
                                $"No configured time providers support freeze.");
                            return;
                        }

                        firstFreezableTimeProvider.Freeze(dateTime);

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

                    var currentTimeProvider = TemporalTime.CurrentTimeProvider;

                    if (currentTimeProvider == null)
                    {
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync(
                            "TemporalTime.CurrentTimeProvider is not currently frozen.");
                        return;
                    }

                    if (!currentTimeProvider.SupportsFreeze)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync(
                            $"Current time provider: {currentTimeProvider.GetType().Name} does not support unfreeze.");
                        return;
                    }

                    currentTimeProvider.Unfreeze();

                    context.Response.StatusCode = 200;

                    await Task.FromResult(true);
                });
            });
        }
    }
}
