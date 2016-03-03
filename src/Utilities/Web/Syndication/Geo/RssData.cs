using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public class RssData
    {
        public RssData(string title, string description, string generator = "JosephGuadagno.net",
            string language = "en-us")
        {
            Header = new SyndicationHeader
            {
                Title = title,
                Description = description,
                Generator = generator,
                Language = language
            };
            Items = new Collection<IGeoItem>();
        }

        public RssData(SyndicationHeader header)
        {
            Header = header;
            Items = new Collection<IGeoItem>();
        }

        public SyndicationHeader Header { get; set; }

        public Collection<IGeoItem> Items { get; }

        public RssPointItem AddPoint(IGeoItem geoItem)
        {
            RssPointItem p = new RssPointItem(geoItem, new Point(geoItem.Latitude, geoItem.Longitude));
            Items.Add(p);
            return p;
        }

        public RssPolygonItem AddPolygon(IGeoItem geoItem, string polygon)
        {
            return AddPolygon(geoItem, ParsePoints(polygon));
        }

        public RssPolygonItem AddPolygon(IGeoItem geoItem, Point[] polygon)
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

            RssPolygonItem p = new RssPolygonItem(geoItem, polygon);
            Items.Add(p);
            return p;
        }

        public RssLineItem AddLine(IGeoItem geoItem, string line)
        {
            return AddLine(geoItem, ParsePoints(line));
        }

        public RssLineItem AddLine(IGeoItem geoItem, Point[] line)
        {
            if (line.Length < 2)
            {
                throw new ArgumentException("A line should contain at least 2 points");
            }
            RssLineItem p = new RssLineItem(geoItem, line);
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

            List<Point> lines = new List<Point>();

            for (int x = 0; x < s.Length + 1/2; x += 2)
            {
                Point p = new Point(double.Parse(s[x], CultureInfo.InvariantCulture),
                    double.Parse(s[x + 1], CultureInfo.InvariantCulture));
                lines.Add(p);
            }
            return lines.ToArray();
        }

        #endregion
    }
}