using RestSharp;
using Services.Responses.SunriseSunset;
using System;
using System.Net;

namespace Services.SunriseSunset
{
    public interface ISunriseSunsetService
    {
        GetSunriseSunsetResult GetSunriseSunset(double latitude, double longitude, DateTime date);
    }

    public class SunriseSunsetService : ISunriseSunsetService
    {
        private readonly string _sunriseSunsetApiUrl;

        public SunriseSunsetService(string sunriseSunsetApiUrl)
        {
            _sunriseSunsetApiUrl = sunriseSunsetApiUrl;
        }

        public GetSunriseSunsetResult GetSunriseSunset(double latitude, double longitude, DateTime date)
        {
            var client = new RestClient(_sunriseSunsetApiUrl);

            var request = new RestRequest("json");
            request.AddParameter("lat", latitude);
            request.AddParameter("lng", longitude);
            request.AddParameter("date", date.ToString("yyyy\\-MM\\-dd"));
            request.AddParameter("formatted", 0); //iso8610 date format

            IRestResponse<SunriseSunsetResponse> response = null;
            try
            {
                response = client.Execute<SunriseSunsetResponse>(request);

                if (response.StatusCode == HttpStatusCode.OK && response.Data?.Status == "OK")
                {
                    return new GetSunriseSunsetResult(
                        response.Data.Results.Sunrise,
                        response.Data.Results.Sunset,
                        response.Data.Results.DayLength);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }
}
