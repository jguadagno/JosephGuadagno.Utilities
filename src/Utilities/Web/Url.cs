using System.Web;

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
                appPath = string.Format("{0}://{1}{2}",
                    context.Request.Url.Scheme,
                    context.Request.Url.Host,
                    context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port);
            }

            return appPath;
        }

        public static string FixupUrl(string url)
        {
            //Getting the current context of HTTP request
            HttpContext context = HttpContext.Current;

            //Checking the current context content
            if (context != null)
            {
                return context.Request.Url.Host != "localhost"
                    ? VirtualPathUtility.ToAbsolute(url).Replace(HttpRuntime.AppDomainAppVirtualPath, "")
                    : VirtualPathUtility.ToAbsolute(url);
            }
            return null;
        }

        public static string FixupUrlWithDomain(string url)
        {
            return FullyQualifiedApplicationPath() + FixupUrl(url);
        }

        public static int ParseRequestParam(string parameter)
        {
            if (string.IsNullOrEmpty(parameter)) return 0;
            int test;
            int.TryParse(parameter, out test);
            return test;
        }
    }
}