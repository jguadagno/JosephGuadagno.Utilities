using System;
using System.Collections.ObjectModel;
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public class RssPolygonItem : RssItem
    {
        public RssPolygonItem(IGeoItem rssItem)
        {
            Copyright = rssItem.Copyright;
            CreatedOn = rssItem.CreatedOn;
            Icon = rssItem.Icon;
            Latitude = rssItem.Latitude;
            Longitude = rssItem.Longitude;
            Summary = rssItem.Summary;
            Title = rssItem.Title;
        }

        public RssPolygonItem(IGeoItem rssItem, Point[] polygon) : this(rssItem)
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

        public Collection<Point> Polygon { get; private set; }
    }
}