using RestSharp;
using Services.Responses.Geocoding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Services.Geocoding
{
    public interface IGeocodingService
    {
        IEnumerable<GetCoordinatesResult> GetCoordinates(string place);
    }

    public class GeocodingService : IGeocodingService
    {
        private readonly string _yahooapisUrl;

        public GeocodingService(string yahooapisUrl)
        {
            _yahooapisUrl = yahooapisUrl;
        }

        public IEnumerable<GetCoordinatesResult> GetCoordinates(string place)
        {
            var client = new RestClient(_yahooapisUrl);
            var request = new RestRequest("v1/public/yql");

            var placeEscaped = place.Replace("\"", "\\\"");
            request.AddParameter("q", $"select * from geo.places where text=\"{placeEscaped}\"");
            request.AddParameter("format", "json");

            IRestResponse<GeocodingResponse> response = null;
            try
            {
                response = client.Execute<GeocodingResponse>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Data?.Query.Results.Place
                        .Select(p => new GetCoordinatesResult(p.Centroid.Latitude, p.Centroid.Longitude));
                }
            }
            catch (System.Exception)
            {
                return null;
            }

            return null;
        }
    }
}
