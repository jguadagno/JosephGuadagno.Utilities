using System;
using System.Web;
using JosephGuadagno.Utilities.Security;

namespace JosephGuadagno.Utilities.Web
{
    public static class Url
    {
        public static string FullyQualifiedApplicationPath()
        {
            //Return variable declaration
            string appPath = null;

            //Getting the current context of HTTP request
            HttpContext context = HttpContext.Current;

            //Checking the current context content
            if (context != null)
            {
                //Formatting the fully qualified website url/name
                appPath =
                    $"{context.Request.Url.Scheme}://{context.Request.Url.Host}{(context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port)}";
            }

            return appPath;
        }

        public static string FixupUrl(string url)
        {
            //Getting the current context of HTTP request
            HttpContext context = HttpContext.Current;

            //Checking the current context content
            if (context == null) return null;

            if (HttpRuntime.AppDomainAppVirtualPath != null)
                return context.Request.Url.Host != "localhost"
                    ? VirtualPathUtility.ToAbsolute(url).Replace(HttpRuntime.AppDomainAppVirtualPath, "")
                    : VirtualPathUtility.ToAbsolute(url);
            return null;
        }

        public static string FixupUrlWithDomain(string url)
        {
            return FullyQualifiedApplicationPath() + FixupUrl(url);
        }

        public static string GetHashedParameterUrl(string page, string idFieldName, string hashFieldName, int id,
            string saltWith)
        {
            var parameters = Hash.GetHashedParameter(idFieldName, hashFieldName, id, saltWith);
            return $"{GetAbsolutePath(page)}?{parameters}";
        }

        public static string GetUrl(string page, string parameter, int id)
        {
            return GetUrl(page, parameter, id.ToString());
        }

        public static string GetUrl(string page, string parameter, string id)
        {
            return $"{GetAbsolutePath(page)}?{parameter}={id}";
        }

        private static string GetAbsolutePath(string page, string defaultRoot = null)
        {
            try
            {
                return VirtualPathUtility.ToAbsolute(page);
            }
            catch (Exception)
            {
                return page.Replace("~", defaultRoot);
            }
        }
    }
}