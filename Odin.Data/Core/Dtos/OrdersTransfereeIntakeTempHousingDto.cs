using System;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeTempHousingDto
    {
        public string Id { get; set; }
        public int TempHousingDays { get; set; }
        public DateTime? TempHousingEndDate { get; set; }
    }
}
