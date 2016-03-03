namespace JosephGuadagno.Utilities.Interfaces
{
    /// <summary>
    ///     Represents the GeoRSS specific items to an RSS feed.
    /// </summary>
    public interface IGeoItem : IRssItem
    {
        /// <summary>
        ///     Gets the latitude of the item.
        /// </summary>
        /// <value>The latitude.</value>
        double Latitude { get; }

        /// <summary>
        ///     Gets the longitude of the item.
        /// </summary>
        /// <value>The longitude.</value>
        double Longitude { get; }

        /// <summary>
        ///     Gets the Url of the icon to be used for the pointer.
        /// </summary>
        /// <value>The icon.</value>
        string Icon { get; }
    }
}