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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace JosephGuadagno.Utilities.GeoRSS
{
    public class GeoRssData
    {
        public GeoRssData(string title, string description)
        {
            Description = description;
            Title = title;
            Items = new Collection<GeoRssItem>();
        }

        public string Title { get; }

        public string Description { get; }

        public Collection<GeoRssItem> Items { get; }


        public GeoRssPointItem AddPoint(string itemTitle, string itemDescription, double latitude, double longitude)
        {
            GeoRssPointItem p = new GeoRssPointItem {Title = itemTitle, Description = itemDescription};
            p.Point = new Point(latitude, longitude);
            Items.Add(p);
            return p;
        }

        public GeoRssPolygonItem AddPolygon(string itemTitle, string itemDescription, string polygon)
        {
            return AddPolygon(itemTitle, itemDescription, ParsePoints(polygon));
        }

        public GeoRssPolygonItem AddPolygon(string itemTitle, string itemDescription, Point[] polygon)
        {
            if (polygon.Length < 3)
            {
                throw new ArgumentException("A polygon should contain at least 3 points");
            }

            if (polygon[0].Latitude != polygon[polygon.Length - 1].Latitude ||
                polygon[0].Longitude != polygon[polygon.Length - 1].Longitude)
            {
                throw new ArgumentException("The last and first points in a polygon should be the same");
            }

            GeoRssPolygonItem p = new GeoRssPolygonItem(polygon) {Title = itemTitle, Description = itemDescription};
            Items.Add(p);
            return p;
        }

        public GeoRssLineItem AddLine(string itemTitle, string itemDescription, string line)
        {
            return AddLine(itemTitle, itemDescription, ParsePoints(line));
        }

        public GeoRssLineItem AddLine(string itemTitle, string itemDescription, Point[] line)
        {
            if (line.Length < 2)
            {
                throw new ArgumentException("A line should contain at least 2 points");
            }
            GeoRssLineItem p = new GeoRssLineItem(line) {Title = itemTitle, Description = itemDescription};
            Items.Add(p);
            return p;
        }

        #region Helpers

        private static Point[] ParsePoints(string line)
        {
            string[] s = line.Split(' ');

            if (s.Length%2 != 0)
            {
                throw new ArgumentException("There must be an even number of points in a line or polygon");
            }

            List<Point> _line = new List<Point>();

            for (int x = 0; x < s.Length + 1/2; x += 2)
            {
                Point p = new Point(double.Parse(s[x], CultureInfo.InvariantCulture),
                    double.Parse(s[x + 1], CultureInfo.InvariantCulture));
                _line.Add(p);
            }
            return _line.ToArray();
        }

        #endregion
    }
}