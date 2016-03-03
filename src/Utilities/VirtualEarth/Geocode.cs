using System.Linq;

namespace JosephGuadagno.Utilities.VirtualEarth
{
    /// <summary>
    /// Contains functions for interacting with the Bing Maps Geo-code functions.
    /// </summary>
    public class Geocode
    {
        /// <summary>
        ///     Gets the location.
        /// </summary>
        /// <param name="appId">The application id.</param>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static GeocodeResponse GetGeocodeResponse(string appId, string address)
        {
            GeocodeRequest geocodeRequest = new GeocodeRequest
            {
                Credentials = new Credentials {ApplicationId = appId},
                Query = address
            };
            ConfidenceFilter[] filters = new ConfidenceFilter[1];
            filters[0] = new ConfidenceFilter {MinimumConfidence = Confidence.High};
            GeocodeOptions geocodeOptions = new GeocodeOptions {Filters = filters};
            geocodeRequest.Options = geocodeOptions;

            GeocodeServiceClient geocodeServiceClient = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService");
            return geocodeServiceClient.Geocode(geocodeRequest);
        }

        public static GeocodeLocation GetFirstLocation(string appId, string address)
        {
            GeocodeResponse response = GetGeocodeResponse(appId, address);

            return !response.Results.Any() ? null : response.Results[0].Locations[0];
        }
    }
}