using System;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeDetailsServiceDto
    {
        public string Id { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Notes { get; set; }
    }
}
