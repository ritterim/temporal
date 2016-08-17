using System;

namespace Temporal
{
    public class CookieTimeProvider : ITimeProvider
    {
        public const string CookieName = "__TemporalTime";

        private readonly CookieService cookieService;

        public CookieTimeProvider(CookieService cookieService)
        {
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

        public DateTime? ReadCookie()
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

        public void RemoveCookie()
        {
            cookieService.Delete(CookieName);
        }

        public void SetCookie(DateTime freezeDateTime)
        {
            cookieService.Append(CookieName, freezeDateTime.ToString("o"));
        }
    }
}
