using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Addons;
using Dapplo.HttpExtensions;
using Dapplo.Log;
using Dapplo.Today.Configuration;
using Ical.Net;

namespace Dapplo.Today.Modules
{
    /// <summary>
    /// Read the calendar
    /// </summary>
    [StartupAction]
    public class CalendarReader : IAsyncStartupAction
    {
        private readonly ICalendarConfiguration _calendarConfiguration;
        private static readonly LogSource Log = new LogSource();
        /// <summary>
        /// The TeamCalendar
        /// </summary>
        public Calendar TeamCalendar { get; private set; }

        [ImportingConstructor]
        public CalendarReader(ICalendarConfiguration calendarConfiguration)
        {
            _calendarConfiguration = calendarConfiguration;
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var iCalTeam = _calendarConfiguration.CalendarUri;

            if (iCalTeam == null)
            {
                Log.Warn().WriteLine("No iCal configured");
                return;
            }
            using (var iCalStream = await iCalTeam.GetAsAsync<Stream>(cancellationToken))
            {
                TeamCalendar = Calendar.Load(iCalStream);
            }

            foreach (var teamEvent in TeamCalendar.Events.Where(teamEvent => teamEvent.IsActive))
            {
                if (teamEvent.Start.Date > DateTime.Now || teamEvent.End.Date < DateTime.Now)
                {
                    continue;
                }
                var attendees = teamEvent.Attendees.Select(attendee => attendee.CommonName);
                Log.Info().WriteLine("{0} - {1}", teamEvent.Summary, string.Join(", ", attendees));
            }
        }
    }
}
