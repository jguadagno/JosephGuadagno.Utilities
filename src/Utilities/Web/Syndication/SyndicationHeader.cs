namespace JosephGuadagno.Utilities.Web.Syndication
{
    /// <summary>
    ///     Provides the header information for a Syndicated feed.
    /// </summary>
    public class SyndicationHeader
    {
        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public string Language { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public string Generator { get; set; }
    }
}