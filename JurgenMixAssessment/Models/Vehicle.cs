namespace JurgenMixAssessment.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string VehicleRegistration { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public ulong RecordedTimeUTC { get; set; }
    }
}
