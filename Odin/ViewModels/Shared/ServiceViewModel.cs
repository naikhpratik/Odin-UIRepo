using System;

namespace Odin.ViewModels.Shared
{
    public class ServiceViewModel
    {
        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string Name { get; set; }
    }
}