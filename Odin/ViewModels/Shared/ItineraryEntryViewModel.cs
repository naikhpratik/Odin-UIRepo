using System;

namespace Odin.ViewModels.Shared
{
    public class ItineraryEntryViewModel
    {
        public string Id { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string ActionLabel { get; set; }
        public string ItemType { get; set; }

        public string DayNumber
        {
            get { return ScheduledDate.HasValue ? ScheduledDate.Value.ToString("dd") : "NA"; }
        }

        public string DayName
        {
            get { return ScheduledDate.HasValue ? ScheduledDate.Value.ToString("dddd") : "NA"; }
        }

        public string MonthYear
        {
            get
            {
                return ScheduledDate.HasValue ? ScheduledDate.Value.ToString("MMMM yyyy") : "NA";
            }
        }

        public string Time
        {
            get { return ScheduledDate.HasValue ? ScheduledDate.Value.ToString("hh:mm tt") : "NA"; }
        }
    }
}