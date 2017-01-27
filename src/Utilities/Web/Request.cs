using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace JosephGuadagno.Utilities.Web
{
    public static class WebRequest
    {
        public static T Get<T>(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var json = client.DownloadString(url);
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception)
            {
                return (default(T));
            }
        }

        public static T Get<T>(string url, Dictionary<string, string> parameters)
        {
            string queryParameters = parameters.Aggregate("",
                (current, parameter) => current + ("&" + parameter.Key + "=" + parameter.Value));
            var fullUrl = url + "?" + queryParameters;
            return Get<T>(fullUrl);
        }
    }
}