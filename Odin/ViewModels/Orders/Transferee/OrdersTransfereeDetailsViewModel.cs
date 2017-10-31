using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Models;


namespace Odin.ViewModels.Orders.Transferee
{
    public class OrdersTransfereeDetailsViewModel
    {
        public OrdersTransfereeDetailsViewModel()
        {            
            Services = new List<ServiceViewModel>();
         }

        public string Id { get; set; }
                
        public DateTime? PreTripDate { get; set; }
        public int TempHousingDays { get; set; }
        public DateTime? TempHousingEndDate { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
        public DateTime? HomeFindingDate { get; set; }

        public DateTime? WorkStartDate { get; set; }
        
        public string TempHousingEndDateDisplay =>
            TempHousingEndDate.HasValue ? TempHousingEndDate.Value.ToString("d") : String.Empty;
        
        public string SchoolDistrict { get; set; }

        
        public IEnumerable<ServiceViewModel> Services { get; set; }
        
        public Rent Rent { get; set; }

        public string ProgramManagerId { get; set; }
        public virtual Manager ProgramManager { get; set; }
        public string SeCustNumb { get; set; }
        public string Client { get; set; }
        public string ClientFileNumber { get; set; }
    }
}