//===============================================================================
// Microsoft Platform Architecture Team
// LitwareHR - S+S Sample Application
//===============================================================================
// Copyright  Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Globalization;

namespace JosephGuadagno.Utilities.GeoRSS
{
    public class Point
    {
        public Point(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Longitude { get; }

        public double Latitude { get; }

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