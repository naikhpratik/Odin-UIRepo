using System;

namespace Odin.Data.Core.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public Order Order { get; set; }

        public ServiceType ServiceType {get; set;}
    }
}
