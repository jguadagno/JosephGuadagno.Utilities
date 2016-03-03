using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;
using JosephGuadagno.Utilities.Extensions;
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication
{
    /// <summary>
    ///     Generates the <c>iCal</c> and VCS files.
    /// </summary>
    public class Calendar
    {
        /// <summary>
        ///     The Default Time Zone to use.
        /// </summary>
        private const string DefaultTimeZone = "America/Los_Angeles";

        /// <summary>
        ///     Generates the calendar.
        /// </summary>
        /// <param name="calendarItem">The calendar item.</param>
        /// <param name="context">The context.</param>
        /// <param name="timeZoneToUse">The time zone to use.</param>
        public static void GenerateCalendar(ICalendarItem calendarItem, HttpContext context,
            string timeZoneToUse = DefaultTimeZone)
        {
            iCalendar iCal = GetiCalendarInstance(timeZoneToUse);

            string fileName = $"{GetSafeFilename(calendarItem.Name)}.vcs";
            AppendEventToCalendar(calendarItem, iCal, timeZoneToUse);
            OutputIcsFile(iCal, context, fileName);
        }

        public static void GenerateCalendar(IEnumerable<ICalendarItem> items, HttpContext context,
            string timeZoneToUse = DefaultTimeZone)
        {
            iCalendar iCal = GetiCalendarInstance(timeZoneToUse);

            items.ForEach(item => AppendEventToCalendar(item, iCal, timeZoneToUse));
            OutputIcsFile(iCal, context);
        }

        private static iCalendar GetiCalendarInstance(string timeZoneToUse)
        {
            iCalendar iCal = new iCalendar
            {
                ProductID = "-//josephguadagno.net//NONSGML josephguadagno.net//EN"
            };
            iCal.AddChild(iCal.GetTimeZone(timeZoneToUse));

            return iCal;
        }

        private static void AppendEventToCalendar(ICalendarItem calendarItem, iCalendar iCal, string timeZoneToUse)
        {
            Event calEvent = iCal.Create<Event>();
            calEvent.Summary = calendarItem.Summary;
            calEvent.Description = calendarItem.Description;
            calEvent.Start = new iCalDateTime(calendarItem.StartDate, timeZoneToUse);
            calEvent.End = new iCalDateTime(calendarItem.EndDate, timeZoneToUse);
            calEvent.Status = calendarItem.Status == "Confirmed" ? EventStatus.Confirmed : EventStatus.Tentative;
            calEvent.GeographicLocation = new GeographicLocation(calendarItem.Latitude, calendarItem.Longitude);
            calEvent.Location = calendarItem.Location ?? "";
            calEvent.Organizer = new Organizer(calendarItem.Organizer);
        }

        private static void OutputIcsFile(iCalendar icalData, HttpContext context, string filename = "calendar.ics")
        {
            HttpResponse response = context.Response;
            response.Clear();
            response.Buffer = true;
            response.ContentType = "text/calendar";
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "utf-8";
            string header = $"attachment;filename=\"{filename}\"";
            response.AddHeader("Content-Disposition", header);
            iCalendarSerializer serializer = new iCalendarSerializer();
            response.Write(serializer.SerializeToString(icalData));
            response.End();
        }

        private static string GetSafeFilename(string name)
        {
            // first trim the raw string
            string safe = name.Trim();

            // replace spaces with hyphens
            safe = safe.Replace(" ", "-").ToLower();

            // replace any 'double spaces' with singles
            if (safe.IndexOf("--", StringComparison.Ordinal) > -1)
                while (safe.IndexOf("--", StringComparison.Ordinal) > -1)
                    safe = safe.Replace("--", "-");

            // trim out illegal characters
            safe = Regex.Replace(safe, "[^a-z0-9\\-]", "");

            // trim the length
            if (safe.Length > 50)
                safe = safe.Substring(0, 49);

            // clean the beginning and end of the filename
            char[] replace = {'-', '.'};
            safe = safe.TrimStart(replace);
            safe = safe.TrimEnd(replace);

            return safe;
        }
    }
}