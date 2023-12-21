// Waypoint.cs
using System.Globalization;

namespace KmlToWorldScript.Classes
{
    public class Waypoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public override string ToString()
        {
            return $"{Latitude.ToString("G", CultureInfo.InvariantCulture)},{Longitude.ToString("G", CultureInfo.InvariantCulture)},{Altitude.ToString("+000000.0;-000000.0", CultureInfo.InvariantCulture)}";
        }
    }
}
