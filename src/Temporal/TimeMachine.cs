using System;

namespace Temporal
{
    public class TimeMachine
    {
        private const string IconImgSrc = "";

        private readonly TemporalOptions _options;

        public TimeMachine(TemporalOptions options)
        {
            _options = options;
        }

        public string GetHtml()
        {
            var css = Resources.GetCss();

            return @"
<!-- Begin TimeMachine -->" + Environment.NewLine +
$@"<style>
    {css}
</style>
<div class=""temporal-time-machine-widget temporal-time-machine-alignment-{_options.TimeMachineAlignment.ToString().ToLowerInvariant()}"">
  <div id=""temporal-time-machine-header-js"" class=""temporal-time-machine-header"">
    <h2 class=""temporal-time-machine-title"">
      <a href=""#"">
        <img class=""temporal-time-machine-icon{(SystemClock.IsFrozen ? " temporal-time-machine-icon-disabled" : null)}"" src=""{IconImgSrc}"" />
        <strong>Time:</strong> {(SystemClock.IsFrozen ? "Frozen" : "Live")}
      </a>
    </h2>
  </div>
  <div id=""temporal-time-machine-collapse-container-js"" class=""temporal-time-machine-body"">
    <ul>
      <li>
        <a href=""#"" class=""temporal-time-machine-item"">
          <h3><strong>SystemClock.UtcNow:</strong> {SystemClock.UtcNow}</h3>
        </a>
      </li>
      <li>
        <a href=""#"" class=""temporal-time-machine-item"">
          <h3><strong>SystemClock.Now:</strong> {SystemClock.Now}</h3>
        </a>
      </li>
      <li>
        <a href=""#"" class=""temporal-time-machine-item temporal-time-machine-last-item"">
          <h3><strong>Change time:</strong> TODO</h3>
        </a>
      </li>
    </ul>
  </div>
</div>
<script>
  (function() {{
    var header = document.getElementById('temporal-time-machine-header-js');
    var collapseContainer = document.getElementById('temporal-time-machine-collapse-container-js');

    collapseContainer.style.display = 'none';

    header.addEventListener('click', function() {{
      var currentDisplay = collapseContainer.style.display;

      if (currentDisplay === 'none') {{
        collapseContainer.style.display = 'inherit';
      }}
      else {{
        collapseContainer.style.display = 'none';
      }}
    }}, false);
  }})();
</script>".Replace(Environment.NewLine, null) + Environment.NewLine +
"<!-- End TimeMachine -->" + Environment.NewLine;
        }
    }
}
