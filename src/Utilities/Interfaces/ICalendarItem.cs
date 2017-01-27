using System;

namespace JosephGuadagno.Utilities.Interfaces
{
    /// <summary>
    ///     Provides an Interface to generate an ICal record.
    /// </summary>
    public interface ICalendarItem
    {
        /// <summary>
        ///     Gets the summary.
        /// </summary>
        /// <value>The summary.</value>
        string Summary { get; }

        /// <summary>
        ///     Gets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        ///     Gets the start date.
        /// </summary>
        /// <value>The start date.</value>
        DateTime StartDate { get; }

        /// <summary>
        ///     Gets the end date.
        /// </summary>
        /// <value>The end date.</value>
        DateTime EndDate { get; }

        /// <summary>
        ///     Gets the location.
        /// </summary>
        /// <value>The location.</value>
        string Location { get; }

        /// <summary>
        ///     Gets the organizer.
        /// </summary>
        /// <value>The organizer.</value>
        string Organizer { get; }

        /// <summary>
        ///     Gets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        double Latitude { get; }

        /// <summary>
        ///     Gets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        double Longitude { get; }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <value>The status.</value>
        string Status { get; }

        /// <summary>
        ///     Gets the name of the calendar item.
        /// </summary>
        string Name { get; }
    }
}