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
using System.Collections.ObjectModel;

namespace JosephGuadagno.Utilities.GeoRSS
{
    public class GeoRssPointItem : GeoRssItem
    {
        public Point Point { set; get; }
    }

    public class GeoRssLineItem : GeoRssItem
    {
        public GeoRssLineItem(Point[] line)
        {
            Line = new Collection<Point>(line);
        }

        public Collection<Point> Line { get; }
    }

    public class GeoRssPolygonItem : GeoRssItem
    {
        public GeoRssPolygonItem(Point[] polygon)
        {
            if (polygon.Length < 3)
            {
                throw new ArgumentException("A polygon must have at least 3 coordinates");
            }

            if (polygon[0].Latitude != polygon[polygon.Length - 1].Latitude ||
                polygon[0].Longitude != polygon[polygon.Length - 1].Longitude)
            {
                throw new ArgumentException("The last and first points in a polygon should be the same");
            }

            Polygon = new Collection<Point>(polygon);
        }

        public Collection<Point> Polygon { get; }
    }

    public class GeoRssItem
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string Icon { set; get; }
        public string Link { set; get; }
    }
}