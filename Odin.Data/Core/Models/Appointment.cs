using System;
using System.ComponentModel.DataAnnotations;

namespace Odin.Data.Core.Models
{
    public class Appointment : MobileTable
    {
        [Display(Name = "Scheduled Date:")]
        [DisplayFormat(DataFormatString = "{0:dddd}", NullDisplayText = "NA")]
        public DateTime? ScheduledDate { get; set; }

        [Display(Name = "Description:")]
        public string Description { get; set; }

        public string OrderId { get; set; }
        public Order Order { get; set; }
        public void Delete()
        {
            Deleted = true;
        }
    }
}
