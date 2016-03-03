using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace JosephGuadagno.Utilities.Web
{
    public static class Http
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
                    $"{context.Request.Url.Scheme}://{context.Request.Url.Host}{(context.Request.Url.Port == 80 ? String.Empty : ":" + context.Request.Url.Port)}";
            }

            return appPath;
        }

        public static string FixupUrl(string Url)
        {
            return Url.StartsWith("http://") ? Url : VirtualPathUtility.ToAbsolute(Url);
        }

        public static string FixupUrlWithDomain(string Url)
        {
            return Url.StartsWith("http://") ? Url : FullyQualifiedApplicationPath() + FixupUrl(Url);
        }

        public static string FixupUrlWithDomain(string url, string parameter, string value)
        {
            return $"{FullyQualifiedApplicationPath() + FixupUrl(url)}?{parameter}={value}";
        }

        public static string FixupUrlWithDomain(string url, string parameter, int value)
        {
            return FixupUrlWithDomain(url, parameter, value.ToString());
        }

        public static string GetUrl(string page, string parameter, int id)
        {
            return GetUrl(page, parameter, id.ToString());
        }

        public static string GetUrl(string page, string parameter, string id)
        {
            return $"{VirtualPathUtility.ToAbsolute(page)}?{parameter}={id}";
        }

        public static string GetUrlForRoute(string routeName, RouteValueDictionary parameters)
        {
            VirtualPathData vpd = RouteTable.Routes.GetVirtualPath(null, routeName, parameters);

            return vpd?.VirtualPath;
        }

        /// <summary>
        ///     Determines if the current client is a Mobile Browser.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Based on Code Project article: http://www.codeproject.com/Articles/34422/Detecting-a-mobile-browser-in-ASP-NET</remarks>
        public static bool IsMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            HttpContext context = HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi",
                    "moto", "iphone", "ipod", "ipad", "android", "rim tablet", "webos"
                };

                //Loop through each item in the list created above 
                //and check if the header contains that text
                return
                    mobiles.Any(
                        s => context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLowerInvariant()));
            }

            return false;
        }
    }
}