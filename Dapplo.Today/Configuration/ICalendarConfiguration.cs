using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Dapplo.Ini;

namespace Dapplo.Today.Configuration
{
    [IniSection("Calendar")]
    public interface ICalendarConfiguration : IIniSection
    {
        [Description("The uri to the ical which describes the Calendar")]
        [DataMember(EmitDefaultValue = true)]
        Uri CalendarUri { get; }
    }
}
