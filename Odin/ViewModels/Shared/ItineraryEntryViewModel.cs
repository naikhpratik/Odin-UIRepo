using System;

namespace Odin.ViewModels.Shared
{
    public class ItineraryEntryViewModel
    {
        public string Id { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string ActionLabel { get; set; }
        public string ItemType { get; set; }
    }
}