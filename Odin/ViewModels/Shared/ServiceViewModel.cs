using System;

namespace Odin.ViewModels.Shared
{
    public class ServiceViewModel
    {
        public string Id { get; set; }

        public DateTime? ScheduledDate { get; set; }
        public string ScheduledDateDisplay => ScheduledDate.HasValue ? ScheduledDate.Value.ToString("d") : String.Empty;
        public string ScheduledTimeDisplay => ScheduledDate.HasValue ? ScheduledDate.Value.ToString("t") : String.Empty;

        public DateTime? CompletedDate { get; set; }
        public string CompletedDateDisplay => CompletedDate.HasValue ? CompletedDate.Value.ToString("d") : String.Empty;

        public bool Selected { get; set; }

        public string Notes { get; set; }

        public string Name { get; set; }
    }
}