using System;
using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;


namespace Odin.ViewModels.Orders.Transferee
{
    public class OrdersTransfereeDetailsViewModel
    {
        //Profile Summary
        //Important Dates
        public DateTime? PreTripDate { get; set; }
        public int TempHousingDays { get; set; }
        public DateTime? TempHousingEndDate { get; set; }

        //Profile Summary
        //Housing Details
        public int? RentId { get; set; }
        public Rent rent { get; set; }
        //schooldistrict?

        //Profile Summary
        //Other Details
        public string ProgramManagerId { get; set; }
        public virtual Manager ProgramManager { get; set; }
        public string SeCustNumb { get; set; }
        public string Rmc { get; set; }
        public string Client { get; set; }

        //Scheduled Services
        public virtual ICollection<Service> Services { get; private set; }
        public bool HasService(int serviceTypeId)
        {
            return Services.Any<Service>(s => s.ServiceTypeId == serviceTypeId);
        }

        public Service GetService(int serviceTypeId)
        {
            return Services.FirstOrDefault<Service>(s => s.ServiceTypeId == serviceTypeId);
        }
        //Notes?

    }
}