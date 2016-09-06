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
                .SetTimeMachineAlignment(TimeMachineAlignment.Left);

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
  <tr><td>CookieTimeProvider(new CookieService()).Now</td><td>{new CookieTimeProvider(new CookieService()).Now}</td></tr>
  <tr><td>CookieTimeProvider(new CookieService()).UtcNow</td><td>{new CookieTimeProvider(new CookieService()).UtcNow}</td></tr>
</table>

<button id=""get-current-js"">Get current</button>
<pre id=""get-current-result-js""></pre>

<script>
  var getCurrent = document.getElementById('get-current-js');
  var currentResult = document.getElementById('get-current-result-js');

  getCurrent.addEventListener('click', function() {{
    fetch('{Constants.TemporalOptions.DefaultTemporalRootPath + Constants.TemporalOptions.CurrentInfoEndpoint}').then(function(res) {{
      res.json().then(function(json) {{
        currentResult.innerText = JSON.stringify(json);
      }});
    }});
  }});
</script>
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
