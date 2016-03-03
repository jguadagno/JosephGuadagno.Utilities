using System.Net;
using Newtonsoft.Json;

namespace JosephGuadagno.Utilities.Web
{
	public static class WebRequest
	{

		public static T MakeJsonServiceRequest<T>(string url)
		{
			using (var client = new WebClient())
			{
				var json = client.DownloadString(url);
			    return JsonConvert.DeserializeObject<T>(json);
			}
		}
	}
}
