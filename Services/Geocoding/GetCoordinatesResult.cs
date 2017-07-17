namespace Services.Geocoding
{
    public class GetCoordinatesResult
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public GetCoordinatesResult() { }

        public GetCoordinatesResult(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
