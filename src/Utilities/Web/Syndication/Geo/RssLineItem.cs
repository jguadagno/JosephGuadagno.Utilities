using System.Collections.Generic;
using System.Collections.ObjectModel;
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public class RssLineItem : RssItem
    {
        public RssLineItem(IGeoItem rssItem)
        {
            Copyright = rssItem.Copyright;
            CreatedOn = rssItem.CreatedOn;
            Icon = rssItem.Icon;
            Latitude = rssItem.Latitude;
            Longitude = rssItem.Longitude;
            Summary = rssItem.Summary;
            Title = rssItem.Title;
        }

        public RssLineItem(IGeoItem rssItem, IList<Point> line) : this(rssItem)
        {
            Line = new Collection<Point>(line);
        }

        public Collection<Point> Line { get; private set; }
    }
}