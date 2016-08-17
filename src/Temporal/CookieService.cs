using Microsoft.Owin;
using System;
using System.Web;

namespace Temporal
{
    public class CookieService
    {
        private readonly Func<IOwinContext> getContext = () => HttpContext.Current?.Request.GetOwinContext();

        public CookieService(Func<IOwinContext> getContext = null)
        {
            if (getContext != null)
            {
                this.getContext = getContext;
            }
        }

        public virtual void Append(string cookieName, string value)
        {
            getContext().Response.Cookies.Append(cookieName, value, new CookieOptions
            {
                HttpOnly = true,
                // TODO: Set secure cookie if operating in a secure environment.
            });
        }

        public virtual void Delete(string cookieName)
        {
            getContext().Response.Cookies.Delete(cookieName, new CookieOptions());
        }

        public virtual string GetValue(string cookieName)
        {
            return getContext().Request.Cookies[cookieName];
        }
    }
}
