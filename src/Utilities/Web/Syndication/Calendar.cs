using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization.iCalendar.Serializers;
using JosephGuadagno.Utilities.Interfaces;

namespace JosephGuadagno.Utilities.Web.Syndication
{
    /// <summary>
    ///     Generates the <c>calendar</c> and VCS files.
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
            var calendar = GetCalendarInstance(timeZoneToUse);

            string fileName = $"{GetSafeFilename(calendarItem.Name)}.vcs";
            AppendEventToCalendar(calendarItem, calendar, timeZoneToUse);
            OutputIcsFile(calendar, context, fileName);
        }

        public static void GenerateCalendar(IEnumerable<ICalendarItem> items, HttpContext context,
            string timeZoneToUse = DefaultTimeZone)
        {
            var calendar = GetCalendarInstance(timeZoneToUse);

            foreach (var calendarItem in items)
            {
                AppendEventToCalendar(calendarItem, calendar, timeZoneToUse);
            }
            OutputIcsFile(calendar, context);
        }

        private static Ical.Net.Calendar GetCalendarInstance(string timeZoneToUse)
        {
            var calendar = new Ical.Net.Calendar();
            calendar.AddTimeZone(new VTimeZone(timeZoneToUse));
            return calendar;
        }

        private static void AppendEventToCalendar(ICalendarItem calendarItem, Ical.Net.Calendar calendar,
            string timeZoneToUse)
        {
            Event calEvent = calendar.Create<Event>();
            calEvent.Summary = calendarItem.Summary;
            calEvent.Description = calendarItem.Description;
            calEvent.Start = new CalDateTime(calendarItem.StartDate, timeZoneToUse);
            calEvent.End = new CalDateTime(calendarItem.EndDate, timeZoneToUse);
            calEvent.Status = calendarItem.Status == "Confirmed" ? EventStatus.Confirmed : EventStatus.Tentative;
            calEvent.GeographicLocation = new GeographicLocation(calendarItem.Latitude, calendarItem.Longitude);
            calEvent.Location = calendarItem.Location ?? "";
            calEvent.Organizer = new Organizer(calendarItem.Organizer);
        }

        private static void OutputIcsFile(Ical.Net.Calendar calendarData, HttpContext context,
            string filename = "calendar.ics")
        {
            HttpResponse response = context.Response;
            response.Clear();
            response.Buffer = true;
            response.ContentType = "text/calendar";
            response.ContentEncoding = Encoding.UTF8;
            response.Charset = "utf-8";
            string header = $"attachment;filename=\"{filename}\"";
            response.AddHeader("Content-Disposition", header);
            CalendarSerializer serializer = new CalendarSerializer();
            response.Write(serializer.SerializeToString(calendarData));
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