using Services.Geocoding;
using Services.SunriseSunset;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.SunriseSunsetWithGeocoding
{
    public interface ISunriseSunsetWithGeocodingService
    {
        /// <summary>
        /// Gets day length based on location.
        /// </summary>
        /// <param name="location">An address or coordinates (latitude longitude - separated with a space).</param>
        /// <param name="date">The date to retrieve day length for.</param>
        GetSunriseSunsetResult GetSunriseSunset(string location, DateTime date);
        Task<GetSunriseSunsetResult> GetSunriseSunsetAsync(string location, DateTime date);
    }

    public class SunriseSunsetWithGeocodingService : ISunriseSunsetWithGeocodingService
    {
        private readonly IGeocodingService _geocodingService;
        private readonly ISunriseSunsetService _sunriseSunsetService;

        public SunriseSunsetWithGeocodingService(IGeocodingService geocodingService, ISunriseSunsetService sunriseSunsetService)
        {
            _geocodingService = geocodingService;
            _sunriseSunsetService = sunriseSunsetService;
        }

        public GetSunriseSunsetResult GetSunriseSunset(string location, DateTime date)
        {
            double lat, lon;

            if (TryParseCoords(location, out lat, out lon))
            {
                return _sunriseSunsetService.GetSunriseSunset(lat, lon, date);
            }

            var getCoordsResult = _geocodingService.GetCoordinates(location);
            if (getCoordsResult != null && getCoordsResult.Any())
            {
                var coords = getCoordsResult.First();

                var result = _sunriseSunsetService.GetSunriseSunset(coords.Latitude, coords.Longitude, date);
                if (result != null)
                {
                    result.MatchingQueryLocations = getCoordsResult.Count();
                }

                return result;
            }

            return null;
        }

        public async Task<GetSunriseSunsetResult> GetSunriseSunsetAsync(string location, DateTime date)
        {
            return await Task.Run(() => GetSunriseSunset(location, date));
        }

        private bool TryParseCoords(string coords, out double lat, out double lon)
        {
            const string singleCoordinatePattern = "-{0,1}\\d+\\.\\d+";

            var regex = new Regex($"^ *({singleCoordinatePattern}) +({singleCoordinatePattern}) *$");
            var match = regex.Match(coords);

            if (match.Success)
            {
                string latStr = match.Groups[1].Value;
                string lonStr = match.Groups[2].Value;

                if (double.TryParse(latStr, NumberStyles.Any, CultureInfo.InvariantCulture, out lat) &&
                    double.TryParse(lonStr, NumberStyles.Any, CultureInfo.InvariantCulture, out lon))
                {
                    return true;
                }
            }

            lat = 0;
            lon = 0;
            return false;
        }
    }
}
