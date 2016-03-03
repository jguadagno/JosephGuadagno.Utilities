using System;
using System.Globalization;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public class Point
    {
        public Point(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public static Point Parse(string coordinates)
        {
            string[] c = coordinates.Split(' ');

            if (c.Length != 2)
            {
                throw new ArgumentException("Insufficient coordinates. There must be at least 2 numbers");
            }

            double latitude;
            double longitude;

            if (!double.TryParse(c[0], NumberStyles.Any, CultureInfo.InvariantCulture, out latitude) ||
                !double.TryParse(c[1], NumberStyles.Any, CultureInfo.InvariantCulture, out longitude))
            {
                throw new ArgumentException("Coordinates could not be parsed into double numbers");
            }

            return new Point(latitude, longitude);
        }

        public override string ToString()
        {
            return Latitude.ToString(CultureInfo.InvariantCulture) + " " +
                   Longitude.ToString(CultureInfo.InvariantCulture);
        }
    }
}