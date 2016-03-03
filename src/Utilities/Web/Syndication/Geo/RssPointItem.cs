using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public class RssPointItem : RssItem
    {
        public RssPointItem(IGeoItem rssItem)
        {
            Copyright = rssItem.Copyright;
            CreatedOn = rssItem.CreatedOn;
            Icon = rssItem.Icon;
            Latitude = rssItem.Latitude;
            Longitude = rssItem.Longitude;
            Summary = rssItem.Summary;
            Title = rssItem.Title;
        }

        public RssPointItem(IGeoItem rssItem, Point point) : this(rssItem)
        {
            Point = point;
        }

        public Point Point { set; get; }
    }
}