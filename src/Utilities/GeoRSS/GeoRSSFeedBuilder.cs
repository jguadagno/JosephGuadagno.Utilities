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
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml.Linq;

namespace JosephGuadagno.Utilities.GeoRSS
{
    public sealed class GeoRssFeedBuilder
    {
        private static readonly Dictionary<Type, Func<GeoRssItem, SyndicationItem>> Formatters = new Dictionary
            <Type, Func<GeoRssItem, SyndicationItem>>
        {
            {typeof (GeoRssLineItem), i => GetLineItem((i as GeoRssLineItem))},
            {typeof (GeoRssPointItem), i => GetPointItem((i as GeoRssPointItem))},
            {typeof (GeoRssPolygonItem), i => GetPolygonItem(i as GeoRssPolygonItem)},
            {typeof (GeoRssItem), i => GetItem(i)}
        };

        private GeoRssFeedBuilder()
        {
        }


        public static SyndicationFeed GetFeed(GeoRssData builder)
        {
            if (builder == null)
            {
                throw new ArgumentException("You must provide an instance if GeoRssData");
            }

            SyndicationFeed feed = new SyndicationFeed(builder.Title, builder.Description, null);

            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (GeoRssItem item in builder.Items)
            {
                items.Add(Formatters[item.GetType()].Invoke(item));
            }

            feed.Items = items;

            return feed;
        }

        private static SyndicationItem GetPointItem(GeoRssPointItem pointItem)
        {
            SyndicationItem sy_item = BuildItem(pointItem.Title, pointItem.Description, pointItem.Icon, pointItem.Link);
            XNamespace grns = "http://www.georss.org/georss";
            XElement x = new XElement(grns + "point", new XAttribute(XNamespace.Xmlns + "georss", grns.NamespaceName),
                pointItem.Point.Latitude.ToString(CultureInfo.InvariantCulture) + " " +
                pointItem.Point.Longitude.ToString(CultureInfo.InvariantCulture));
            sy_item.ElementExtensions.Add(new SyndicationElementExtension(x));
            return sy_item;
        }

        private static SyndicationItem GetPolygonItem(GeoRssPolygonItem polygonItem)
        {
            SyndicationItem sy_item = BuildItem(polygonItem.Title, polygonItem.Description, polygonItem.Icon,
                polygonItem.Link);
            XNamespace grns = "http://www.georss.org/georss";
            XElement x = new XElement(grns + "polygon", new XAttribute(XNamespace.Xmlns + "georss", grns.NamespaceName),
                GetSequenceOfPoints(polygonItem.Polygon));
            sy_item.ElementExtensions.Add(new SyndicationElementExtension(x));
            return sy_item;
        }

        private static SyndicationItem GetLineItem(GeoRssLineItem lineItem)
        {
            SyndicationItem sy_item = BuildItem(lineItem.Title, lineItem.Description, lineItem.Icon, lineItem.Link);
            XNamespace grns = "http://www.georss.org/georss";
            XElement x = new XElement(grns + "line", new XAttribute(XNamespace.Xmlns + "georss", grns.NamespaceName),
                GetSequenceOfPoints(lineItem.Line));
            sy_item.ElementExtensions.Add(new SyndicationElementExtension(x));
            return sy_item;
        }

        private static SyndicationItem GetItem(GeoRssItem i)
        {
            SyndicationItem sy_item = BuildItem(i.Title, i.Description, i.Icon, i.Link);
            return sy_item;
        }

        private static SyndicationItem BuildItem(string title, string description, string icon, string link)
        {
            SyndicationItem sy_item = new SyndicationItem(title, description, null);
            if (!string.IsNullOrEmpty(icon))
            {
                sy_item.ElementExtensions.Add(AddIcon(icon));
            }
            if (!string.IsNullOrEmpty(link))
            {
                sy_item.ElementExtensions.Add(AddLink(link));
            }

            return sy_item;
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


        private static string GetSequenceOfPoints(Collection<Point> line)
        {
            StringBuilder xml = new StringBuilder();

            foreach (Point x in line)
            {
                xml.Append(x + " ");
            }

            return xml.ToString();
        }
    }
}