using System;

namespace JosephGuadagno.Utilities.Interfaces
{
    /// <summary>
    ///     Represents an item to be included in a RSS feed.
    /// </summary>
    public interface IRssItem
    {
        /// <summary>
        ///     Gets or sets the summary of the item.
        /// </summary>
        /// <value>The summary.</value>
        string Summary { get; }

        /// <summary>
        ///     Gets or sets the title of the item.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; }

        /// <summary>
        ///     Gets or sets the URL of the item.
        /// </summary>
        /// <value>The URL.</value>
        string Url { get; }

        /// <summary>
        ///     Gets or sets when the item was created.
        /// </summary>
        /// <value>The date the item was created.</value>
        DateTime CreatedOn { get; }

        /// <summary>
        ///     Gets or sets when the item was last updated.
        /// </summary>
        /// <value>The date the item was updated.</value>
        DateTime UpdatedOn { get; }

        /// <summary>
        ///     Gets or sets the copyright of the item.
        /// </summary>
        /// <value>The copyright.</value>
        string Copyright { get; }
    }
}