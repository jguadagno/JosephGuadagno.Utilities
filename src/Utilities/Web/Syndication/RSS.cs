using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using JosephGuadagno.Utilities.Interfaces;
using JosephGuadagno.Utilities.Web.Syndication.Geo;

namespace JosephGuadagno.Utilities.Web.Syndication
{
    /// <summary>
    ///     The type of syndicated feed to return
    /// </summary>
    public enum FeedType
    {
        /// <summary>
        ///     Atom Feed
        /// </summary>
        Atom,

        /// <summary>
        ///     RSS feed
        /// </summary>
        Rss,

        /// <summary>
        ///     Geo Feed
        /// </summary>
        Geo
    }

    public static class RSS
    {
        private static string OutputXml(SyndicationFeedFormatter formatter)
        {
            StringBuilder output = new StringBuilder();
            XmlWriter xml = XmlWriter.Create(output);
            formatter.WriteTo(xml);
            xml.Flush();
            return output.ToString();
        }

        public static SyndicationFeed GetFeed(SyndicationHeader header, IEnumerable<IRssItem> items)
        {
            var feed = GetSyndicationHeader(header);
            feed.Items = GetFeedItems(items);
            return feed;
        }

        public static SyndicationFeed GetFeed(SyndicationHeader header, IEnumerable<IGeoItem> items)
        {
            RssData rssData = new RssData(header);

            foreach (var geoItem in items)
            {
                rssData.AddPoint(geoItem);
            }
            return RssFeedBuilder.GetFeed(rssData);
        }

        public static SyndicationFeed GetSyndicationHeader(SyndicationHeader header)
        {
            return new SyndicationFeed
            {
                Title = new TextSyndicationContent(header.Title),
                Description = new TextSyndicationContent(header.Description),
                Language = header.Language,
                Generator = header.Generator
            };
        }

        private static IEnumerable<SyndicationItem> GetFeedItems(IEnumerable<IRssItem> items)
        {
            var rssItems = new List<SyndicationItem>();
            if (items == null) return rssItems;

            foreach (var item in items)
            {
                var rssItem = new SyndicationItem(item.Title, item.Summary,
                    new Uri(Http.FixupUrlWithDomain(item.Url)))
                {
                    PublishDate = item.CreatedOn,
                    LastUpdatedTime = item.UpdatedOn,
                    Copyright = new TextSyndicationContent(item.Copyright)
                };
                rssItems.Add(rssItem);
            }
            return rssItems;
        }

        // Step 1: Create the SyndicationFeed
        // Step 2: Add Feed Item: SyndicationItem
        // Step 3: Format with *FeedFormatter

        #region GenerateFeed

        public static string GenerateFeed(SyndicationHeader header, IEnumerable<IRssItem> items)
        {
            return GenerateFeed(header, items, FeedType.Rss);
        }

        public static string GenerateFeed(SyndicationHeader header, IEnumerable<IRssItem> items, FeedType feedType)
        {
            SyndicationFeedFormatter formatter;

            switch (feedType)
            {
                case FeedType.Atom:
                    formatter = new Atom10FeedFormatter(GetFeed(header, items));
                    break;

                case FeedType.Geo:
                case FeedType.Rss:
                default:
                    formatter = new Rss20FeedFormatter(GetFeed(header, items));
                    break;
            }

            return OutputXml(formatter);
        }

        public static string GenerateFeed(SyndicationHeader header, IEnumerable<IGeoItem> items)
        {
            SyndicationFeedFormatter formatter = new Rss20FeedFormatter(GetFeed(header, items));
            return OutputXml(formatter);
        }

        #endregion GenerateFeed
    }
}