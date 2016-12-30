using System;

namespace Temporal
{
    public class CookieTimeProvider : ITimeProvider
    {
        public const string CookieName = "__TemporalTime";

        private readonly CookieService cookieService;

        public CookieTimeProvider(CookieService cookieService)
        {
            if (cookieService == null) throw new ArgumentNullException(nameof(cookieService));

            this.cookieService = cookieService;
        }

        public DateTime? Now
        {
            get
            {
                var cookieValue = ReadCookie();

                if (cookieValue.HasValue)
                {
                    return cookieValue.Value.ToLocalTime();
                }

                return null;
            }
        }

        public DateTime? UtcNow
        {
            get
            {
                var cookieValue = ReadCookie();

                if (cookieValue.HasValue)
                {
                    return cookieValue.Value.ToUniversalTime();
                }

                return null;
            }
        }

        public bool SupportsFreeze => true;

        public bool IsFrozen => cookieService.GetValue(CookieName) != null;

        public void Freeze(DateTime freezeDateTime)
        {
            cookieService.Append(CookieName, freezeDateTime.ToString("o"));
        }

        public void Unfreeze()
        {
            cookieService.Delete(CookieName);
        }

        private DateTime? ReadCookie()
        {
            var cookie = cookieService.GetValue(CookieName);

            if (string.IsNullOrEmpty(cookie))
            {
                return null;
            }

            DateTime dateTime;
            if (DateTime.TryParse(cookie, out dateTime))
            {
                return dateTime;
            }

            return null;
        }
    }
}
