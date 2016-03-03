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
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication.Geo
{
    public class RssItem : IGeoItem
    {
        public string Title { set; get; }

        /// <summary>
        ///     Gets or sets the summary of the item.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get; set; }

        /// <summary>
        ///     Gets or sets the URL of the item.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets when the item was created.
        /// </summary>
        /// <value>The date the item was created.</value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        ///     Gets or sets when the item was last updated.
        /// </summary>
        /// <value>The date the item was updated.</value>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the copyright of the item.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright { get; set; }

        /// <summary>
        ///     Gets the latitude of the item.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        ///     Gets the longitude of the item.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        ///     Gets the Url of the icon to be used for the pointer.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon { set; get; }
    }
}