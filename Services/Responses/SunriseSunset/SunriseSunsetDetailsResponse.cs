using RestSharp.Deserializers;
using System;

namespace Services.Responses.SunriseSunset
{
    public class SunriseSunsetDetailsResponse
    {
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }

        [DeserializeAs(Name = "day_length")]
        public int DayLength { get; set; }
    }
}
