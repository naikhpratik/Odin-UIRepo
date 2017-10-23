using System;

namespace Odin.ViewModels.Shared
{
    public class ServiceViewModel
    {
        public string Id { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public bool Selected { get; set; }

        public string Notes { get; set; }

        public string Name { get; set; }
    }
}