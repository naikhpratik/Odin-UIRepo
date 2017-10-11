using System;

namespace Odin.Data.Core.Dtos
{
    public class ServiceDto
    {
        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int ServiceTypeId { get; set; }
    }
}
