using Odin.Helpers;
using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odin.ViewModels.Orders.Index
{
    public class OrdersIndexViewModel
    {
        public string Id { get; set; }
        public string SeCustNumb { get; set; }
        public string Rmc { get; set; }
        public string Client { get; set; }
        public DateTime? PreTripDate { private get; set; }
        public DateTime? EstimatedArrivalDate { private get; set; }
        public DateTime? LastContactedDate { private get; set; }
        public bool IsRush { private get; set; }

        public string PreTripDateDisplay => DateHelper.GetViewFormat(PreTripDate);
        public string EstimatedArrivalDateDisplay => DateHelper.GetViewFormat(EstimatedArrivalDate);
        public string LastContactedDateDisplay => DateHelper.GetViewFormat(LastContactedDate);
        public string IsRushDisplay => IsRush ? "Rush" : String.Empty;
        public int AuthorizedServicesDisplay => Services.Count();
        public int ScheduledServicesDisplay => Services.Where(s => s.ScheduledDate.HasValue && !s.CompletedDate.HasValue).Count();
        public int CompletedServicesDisplay => Services.Where(s => s.CompletedDate.HasValue).Count();

        public TransfereeViewModel Transferee { get; set; }

        public ManagerViewModel ProgramManager { get; set; }

        public IEnumerable<ServiceViewModel> Services {get; set;}

    }
}