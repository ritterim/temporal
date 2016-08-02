using Owin;
using System;
using System.Threading.Tasks;
using Temporal;

namespace UsageSample
{
    public class Startup
    {
        public static readonly TemporalOptions TemporalOptions = new TemporalOptions();

        public void Configuration(IAppBuilder app)
        {
            TemporalOptions
                .SetTimeMachineAlignment(TimeMachineAlignment.Right);

            app.UseTemporal(TemporalOptions);

            app.Map("", root =>
            {
                root.Run(context =>
                {
                    context.Response.ContentType = "text/html";

                    context.Response.WriteAsync(
@"<!DOCTYPE html>
<html>
  <head>
    <meta charset=""utf-8"">
    <title>Temporal - UsageSample</title>
  </head>
<body>");

                    context.Response.WriteAsync(
$@"
<table>
  <tr><td>DateTime.Now</td><td>{DateTime.Now}</td></tr>
  <tr><td>DateTime.UtcNow</td><td>{DateTime.UtcNow}</td></tr>
  <tr><td>SystemClock.Now</td><td>{SystemClock.Now}</td></tr>
  <tr><td>SystemClock.UtcNow</td><td>{SystemClock.UtcNow}</td></tr>
</table>
");

                    context.Response.WriteAsync(
                        new TimeMachine(TemporalOptions).GetHtml());

                    context.Response.WriteAsync(
@"</body>
</html>");

                    return Task.FromResult(true);
                });
            });
        }
    }
}
