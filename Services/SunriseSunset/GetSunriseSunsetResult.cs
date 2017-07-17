using System;

namespace Services.SunriseSunset
{
    public class GetSunriseSunsetResult
    {
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public TimeSpan DayLength { get; set; }

        public int MatchingQueryLocations { get; set; }

        public GetSunriseSunsetResult()
        {
            MatchingQueryLocations = 1;
        }

        public GetSunriseSunsetResult(DateTime sunrise, DateTime sunset, int dayLength)
            : this()
        {
            Sunrise = sunrise;
            Sunset = sunset;
            DayLength = TimeSpan.FromSeconds(dayLength);
        }
    }
}
