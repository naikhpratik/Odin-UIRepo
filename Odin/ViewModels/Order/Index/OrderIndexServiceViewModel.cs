using System;

namespace Odin.ViewModels.Order.Index
{
    public class OrderIndexServiceViewModel
    {
        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string Name { get; set; }
    }
}