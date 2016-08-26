using System;

namespace Temporal
{
    public class TimeMachine
    {
        private const string DateTimeFormat = "yyyy-MM-ddThh:mm:ss";
        private const string IconImgSrc = "";

        private readonly TemporalOptions _options;

        public TimeMachine(TemporalOptions options)
        {
            _options = options;
        }

        public string GetHtml()
        {
            var css = Resources.GetCss();
            var js = Resources.GetJs();

            return $@"
<!-- Begin Temporal TimeMachine -->
<style>
{css}
</style>
<div id=""temporal-time-machine-widget-js""
     class=""temporal-time-machine-widget temporal-time-machine-alignment-{_options.TimeMachineAlignment.ToString().ToLowerInvariant()}""
     data-temporal-time-machine-freeze-endpoint=""{Constants.TemporalOptions.DefaultTemporalRootPath + Constants.TemporalOptions.FreezeEndpoint}""
     data-temporal-time-machine-unfreeze-endpoint=""{Constants.TemporalOptions.DefaultTemporalRootPath + Constants.TemporalOptions.UnfreezeEndpoint}"">
  <div id=""temporal-time-machine-header-js""
       class=""temporal-time-machine-header"">
    <h2 class=""temporal-time-machine-title"">
      <a href=""#"">
        <img src=""{IconImgSrc}""
             class=""temporal-time-machine-icon{(TemporalTime.IsFrozen ? " temporal-time-machine-icon-disabled" : null)}"" />
        <strong>Time:</strong> <span id=""temporal-time-machine-header-is-frozen-js"">{(TemporalTime.IsFrozen ? "Frozen" : "Live")}</span>
      </a>
    </h2>
  </div>
  <div id=""temporal-time-machine-collapse-container-js""
       class=""temporal-time-machine-body"">
    <div>
      Local (UTC Offset: {TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours})
      <input id=""temporal-time-machine-local-datetime-js""
             type=""datetime-local""
             value=""{TemporalTime.Now.ToString(DateTimeFormat)}"" />
    </div>
    <div>
      UTC
      <input id=""temporal-time-machine-utc-datetime-js""
             type=""datetime-local""
             value=""{TemporalTime.UtcNow.ToString(DateTimeFormat)}"" />
    </div>
    <div class=""action"">
      <button id=""temporal-time-machine-action-button-js"">
        {(TemporalTime.IsFrozen ? "Go Live" : "Freeze")}
      </button>
    </div>
  </div>
</div>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/fetch/1.0.0/fetch.min.js""></script>
<script>
{js}
</script>
<!-- End Temporal TimeMachine -->
";
        }
    }
}
