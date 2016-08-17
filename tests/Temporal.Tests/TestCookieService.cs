namespace Temporal.Tests
{
    public class TestCookieService : CookieService
    {
        private readonly string cookieValueToReturn;

        public TestCookieService(string cookieValueToReturn)
        {
            this.cookieValueToReturn = cookieValueToReturn;
        }

        public override void Append(string cookieName, string value)
        {
        }

        public override string GetValue(string cookieName)
        {
            return cookieValueToReturn;
        }

        public override void Delete(string cookieName)
        {
        }
    }
}
