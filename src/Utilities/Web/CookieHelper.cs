using System;
using System.Web;

namespace JosephGuadagno.Utilities.Web
{
    public static class CookieHelper
    {
        public static bool CookieExists(string cookieName)
        {
            if (HttpContext.Current == null) return false;
            if (string.IsNullOrEmpty(cookieName)) return false;
            return (HttpContext.Current.Request.Cookies[cookieName] != null);
        }

        public static string GetCookieValue(string cookieName)
        {
            if (HttpContext.Current == null) return string.Empty;
            if (string.IsNullOrEmpty(cookieName)) return string.Empty;
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                return string.Empty;
            }
            var httpCookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            return httpCookie != null ? httpCookie.Value : null;
        }

        public static void SetCookie(string cookieName, string cookieValue)
        {
            if (string.IsNullOrEmpty(cookieName) || string.IsNullOrEmpty(cookieValue)) return;
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue) {HttpOnly = true};
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void SetCookie(string cookieName, string cookieValue, DateTime expireOn)
        {
            if (string.IsNullOrEmpty(cookieName) || string.IsNullOrEmpty(cookieValue)) return;
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue) {HttpOnly = true, Expires = expireOn};
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void ExpireCookie(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName)) return;
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName, string.Empty)
                {HttpOnly = true, Expires = DateTime.Now.AddYears(-5)};
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}