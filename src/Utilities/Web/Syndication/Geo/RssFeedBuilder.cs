using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml.Linq;
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public static class RssFeedBuilder
    {
        private static readonly Dictionary<Type, Func<IGeoItem, SyndicationItem>> Formatters = new Dictionary
            <Type, Func<IGeoItem, SyndicationItem>>
        {
            {typeof (RssLineItem), i => GetLineItem((i as RssLineItem))},
            {typeof (RssPointItem), i => GetPointItem((i as RssPointItem))},
            {typeof (RssPolygonItem), i => GetPolygonItem(i as RssPolygonItem)},
            {typeof (RssItem), i => GetItem(i as RssItem)}
        };


        public static SyndicationFeed GetFeed(RssData builder)
        {
            if (builder == null)
            {
                throw new ArgumentException("You must provide an instance if RssData");
            }

            SyndicationFeed feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent(builder.Header.Title),
                Description = new TextSyndicationContent(builder.Header.Description),
                Language = builder.Header.Language,
                Generator = builder.Header.Generator
            };

            List<SyndicationItem> items = builder.Items.Select(item => Formatters[item.GetType()].Invoke(item)).ToList();

            feed.Items = items;

            return feed;
        }

        private static SyndicationItem BuildItem(string title, string summary, string url, DateTime createdOn,
            DateTime updatedOn, string copyright, string icon)
        {
            SyndicationItem item = new SyndicationItem
            {
                Title = new TextSyndicationContent(title),
                Summary = new TextSyndicationContent(summary),
                PublishDate = createdOn,
                LastUpdatedTime = updatedOn,
                Copyright = new TextSyndicationContent(copyright)
            };
            item.Links.Add(new SyndicationLink(new Uri(url)));

            if (!string.IsNullOrEmpty(icon))
            {
                item.ElementExtensions.Add(AddIcon(icon));
            }
            if (!string.IsNullOrEmpty(url))
            {
                item.ElementExtensions.Add(AddLink(url));
            }

            return item;
        }


        private static SyndicationElementExtension AddLink(string link)
        {
            return AddElementExtensionItem("link", link);
        }

        private static SyndicationElementExtension AddIcon(string icon)
        {
            return AddElementExtensionItem("icon", icon);
        }

        private static SyndicationElementExtension AddElementExtensionItem(string name, string value)
        {
            XElement x = new XElement(name, value);
            return new SyndicationElementExtension(x);
        }

        private static string GetSequenceOfPoints(IEnumerable<Point> line)
        {
            StringBuilder xml = new StringBuilder();

            foreach (Point x in line)
            {
                xml.Append(x + " ");
            }

            return xml.ToString();
        }

        #region Get*Item

        private static SyndicationItem GetPointItem(RssPointItem geoItem)
        {
            SyndicationItem item = BuildItem(geoItem.Title, geoItem.Summary, geoItem.Url, geoItem.CreatedOn,
                geoItem.UpdatedOn, geoItem.Copyright, geoItem.Icon);
            XNamespace grns = "http://www.georss.org/georss";
            XElement x = new XElement(grns + "point", new XAttribute(XNamespace.Xmlns + "georss", grns.NamespaceName),
                geoItem.Point.Latitude.ToString(CultureInfo.InvariantCulture) + " " +
                geoItem.Point.Longitude.ToString(CultureInfo.InvariantCulture));
            item.ElementExtensions.Add(new SyndicationElementExtension(x));
            return item;
        }

        private static SyndicationItem GetPolygonItem(RssPolygonItem geoItem)
        {
            SyndicationItem item = BuildItem(geoItem.Title, geoItem.Summary, geoItem.Url, geoItem.CreatedOn,
                geoItem.UpdatedOn, geoItem.Copyright, geoItem.Icon);
            XNamespace grns = "http://www.georss.org/georss";
            XElement x = new XElement(grns + "polygon", new XAttribute(XNamespace.Xmlns + "georss", grns.NamespaceName),
                GetSequenceOfPoints(geoItem.Polygon));
            item.ElementExtensions.Add(new SyndicationElementExtension(x));
            return item;
        }

        private static SyndicationItem GetLineItem(RssLineItem geoItem)
        {
            SyndicationItem item = BuildItem(geoItem.Title, geoItem.Summary, geoItem.Url, geoItem.CreatedOn,
                geoItem.UpdatedOn, geoItem.Copyright, geoItem.Icon);
            XNamespace grns = "http://www.georss.org/georss";
            XElement x = new XElement(grns + "line", new XAttribute(XNamespace.Xmlns + "georss", grns.NamespaceName),
                GetSequenceOfPoints(geoItem.Line));
            item.ElementExtensions.Add(new SyndicationElementExtension(x));
            return item;
        }

        private static SyndicationItem GetItem(RssItem geoItem)
        {
            SyndicationItem item = BuildItem(geoItem.Title, geoItem.Summary, geoItem.Url, geoItem.CreatedOn,
                geoItem.UpdatedOn, geoItem.Copyright, geoItem.Icon);
            return item;
        }

        #endregion Get*Item
    }
}